using FsTask.ApplicationServices;
using FsTask.Domain;
using FsTask.Domain.Contract;
using Npgsql;
using QuestDB;

namespace FsTask.QuestDB;

public class SensorEventsToGuestDbService : ISensorEventsService
{
    private readonly QuestDbConfig _config;
    const string humanEnvironmentalStatistics = "human_environmental_statistics";

    public SensorEventsToGuestDbService(QuestDbConfig config)
    {
        _config = config;
    }

    public async Task Save(StoreSensorDataCommand cmd, string at)
    {
        var eventsAggregate = new SensorReceivedEventsAggregate(Guid.NewGuid().ToString(), long.Parse(at), cmd);

        using var ls = await GetLineTcpSender();

        foreach (var statistic in eventsAggregate.HumanEnvironmentalStatistics)
        {
            ls.Table(humanEnvironmentalStatistics)
                .Column("human_id", statistic.HumanId)
                .Column("pos_x", statistic.Position.X)
                .Column("pos_y", statistic.Position.Y)
                .Column("vel_x", statistic.Velocity.Y)
                .Column("vel_y", statistic.Velocity.Y)
                .At(eventsAggregate.AtDateTime());
        }

        await ls.SendAsync();
    }


    public async Task<SensorApiDataViewModel> Get(string from, string to)
    {
        var sensorData = new List<InstanceViewModel>();

        var sql = $"select * from '{humanEnvironmentalStatistics}' WHERE timestamp >= {from} AND timestamp <= {to};";

        await using (var reader = await Fetch(sql))
        {
            while (await reader.ReadAsync())
            {
                sensorData.Add(new InstanceViewModel
                {
                    HumanId = reader.GetString(1),
                    PositionX = reader.GetDouble(2),
                    PositionY = reader.GetDouble(3),
                    VelX = reader.GetDouble(4),
                    VelY = reader.GetDouble(5)
                });
            }
        }

        return new SensorApiDataViewModel
        {
            SensorData = new List<SensorDataViewModel>
            {
                new()
                {
                    Instances = sensorData,
                }
            }
        };
    }


    public async Task<SensorDataViewModel> Get(string from)
    {
        throw new NotImplementedException();
    }

    public async Task<DashboardReportViewModel> GetReport(long from, long to, string filter)
    {
        DashboardReportViewModel result = new DashboardReportViewModel();

        var sql = $"select {filter} , timestamp  from 'human_environmental_statistics' WHERE timestamp >= {from} AND timestamp <= {to};";


        await using var reader = await Fetch(sql);
        while (await reader.ReadAsync())
        {
            Add(result, reader.GetDateTime(1).Ticks, reader.GetString(0));
        }

        return result;
    }

    private static void Add(DashboardReportViewModel result, long timeStamp, string yAxis)
    {
        var item = result.Items.FirstOrDefault(a => a.Timestamp == timeStamp);
        if (item != null)
            item.YAxisDatas.Add(yAxis);
        else
            result.Items.Add(new DashboardReportViewModelItem
            {
                Timestamp = timeStamp,
                YAxisDatas = new List<string> { yAxis }
            });
    }

    private async Task<LineTcpSender> GetLineTcpSender()
    {
        return await LineTcpSender.ConnectAsync(_config.Host, _config.Port, tlsMode: TlsMode.Disable);
    }

    private async Task<NpgsqlDataReader> Fetch(string sql)
    {
        await using var connection = new NpgsqlConnection(NpsqlConnectionString());
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(sql, connection);

        return await command.ExecuteReaderAsync();
    }

    private string NpsqlConnectionString()
    {
        return $@"host=localhost;port={_config.NpsqlPort};username={_config.UserName};password={_config.Password};database={_config.DataBase};ServerCompatibilityMode=NoTypeLoading;";
    }
}