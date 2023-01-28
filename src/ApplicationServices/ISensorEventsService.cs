using FsTask.Domain;
using FsTask.Domain.Contract;

namespace FsTask.ApplicationServices;

public interface ISensorEventsService
{
    Task Save(StoreSensorDataCommand cmd, string at);
    Task<SensorApiDataViewModel> Get(string from, string to);

    Task<SensorDataViewModel> Get(string from);
    Task<DashboardReportViewModel> GetReport(long from, long to, string filter);
}

public class SensorEventsService : ISensorEventsService
{
    readonly Dictionary<string, SensorReceivedEventsAggregate> Dictionary = new();

    public Task Save(StoreSensorDataCommand cmd, string at)
    {
        var aggregate =
            new SensorReceivedEventsAggregate(Guid.NewGuid().ToString(), long.Parse(at), cmd);

        Dictionary.Add(at, aggregate);
        return Task.CompletedTask;
    }
    
    public async Task<SensorDataViewModel> Get(string from)
    {
        if (Dictionary.TryGetValue(from, out var a))
        {
            return ToSensorDataViewModel(from, a);
        }

        throw new Exception();
    }

    public async Task<DashboardReportViewModel> GetReport(long from, long to, string filter)
    {
        DashboardReportViewModel result = new DashboardReportViewModel();

        var filteredData = Dictionary
            .OrderBy(a => a.Key)
            .Where(a => long.Parse(a.Key) >= from && long.Parse(a.Key) <= to);

        if (string.IsNullOrWhiteSpace(filter))
            filter = ReportFilter.Human;

        switch (filter)
        {
            case ReportFilter.Human:
                foreach (var (key, value) in filteredData)
                {
                    result.Items.Add(
                    new DashboardReportViewModelItem
                    {
                        Timestamp = long.Parse(key),
                        YAxisDatas = value.HumanEnvironmentalStatistics
                            .GetHumans()
                    });
                }
                break;
            case ReportFilter.PositionX:
                foreach (var (key, value) in filteredData)
                {
                    result.Items.Add(
                    new DashboardReportViewModelItem
                    {
                        Timestamp = long.Parse(key),
                        YAxisDatas = value.HumanEnvironmentalStatistics
                            .GetPositionXs()
                    });
                }
                break;
            case ReportFilter.PositionY:
                foreach (var (key, value) in filteredData)
                {
                    result.Items.Add(
                        new DashboardReportViewModelItem
                        {
                            Timestamp = long.Parse(key),
                            YAxisDatas = value.HumanEnvironmentalStatistics
                                .GetPositionYs()
                        });
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(filter);
        }

        return result;
    }

    public async Task<SensorApiDataViewModel> Get(string from, string to)
    {
        return new SensorApiDataViewModel
        {
            SensorData =
                Dictionary
                .OrderBy(a => a.Key)
                .Select(a => ToSensorDataViewModel(a.Key, a.Value)).ToList(),
            //_links = GenerateHyperLinks(from, to)
        };

    }

    private Links GenerateHyperLinks(string from, string to)
    {
        // TODO links, needs to be adjusted!
        return new Links
        {
            _self =
                new Link
                {
                    _url = $"api/sensor?from={from}&to={to}",
                    _type = "get"
                },
            _hasPreview = true,
            _preview = new Link
            {
                _url = $"api/sensor?from{fromOfPreviewPage(from, to)}&to={toOfPreviewPage(from)}",
                _type = "get"
            },
            _hasNext = true,
            _next = new Link
            {
                _url = $"api/sensor?from{fromOfNextPage(to)}&to={toOfNextPage(from, to)}",
                _type = "get"
            },
            _total = Dictionary.Count
        };
    }

    private string toOfNextPage(string from, string to)
    {
        return (Distance(from, to) + long.Parse(to) + 1).ToString();
    }

    private string fromOfNextPage(string to)
    {
        return (long.Parse(to) + 1).ToString();
    }

    private static long toOfPreviewPage(string from)
    {
        return long.Parse(from) - 1;
    }

    private static long fromOfPreviewPage(string from, string to)
    {
        return long.Parse(from) - Distance(from, to);
    }

    private static long Distance(string from, string to)
    {
        return (long.Parse(to) - long.Parse(from));
    }

    private static SensorDataViewModel ToSensorDataViewModel(string timeStamp, SensorReceivedEventsAggregate a)
    {
        return new SensorDataViewModel
        {
            TimeStamp = timeStamp,
            Instances =

                a.HumanEnvironmentalStatistics
                    .Select(b => new InstanceViewModel
                    {
                        PositionX = b.Position.X,
                        PositionY = b.Position.Y,
                        VelY = b.Velocity.Y,
                        VelX = b.Velocity.X,
                        HumanId = b.HumanId,
                    }).ToList()
        };
    }


}