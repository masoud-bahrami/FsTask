using FsTask.AcceptanceTests.Helpers;
using FsTask.API;
using FsTask.ApplicationServices;
using FsTask.Domain.Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace FsTask.AcceptanceTests.StepDefinitions.Drivers;

class SensorDataApiDriver : ISensorDataDriver
{
    private readonly HttpClient _httpClient;

    public SensorDataApiDriver()
    {
        var webBuilder = new WebHostBuilder();

        const string fsTaskApiAssembly = "FsTask.API, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        webBuilder

            .UseStartup<AcceptanceTestStartup>()

            .UseSetting(WebHostDefaults.ApplicationKey, fsTaskApiAssembly)
            ;

        var _server = new TestServer(webBuilder);

        _httpClient = _server.CreateClient();
    }

    public async Task SendData(string timestamp, Table eventTable)
        => await PutSensorData(timestamp, SensorDataAbstractFactory.CreateCommand(eventTable));


    public async Task SendData(string timestamp)
        => await PutSensorData(timestamp, SensorDataAbstractFactory.CreateCommand());

    public async Task AssertTheOrderOfEvents(Table table)
    {
        (await FetchListOfTimeStampOfStoredSensorEvents()).SequenceEqual(ListOfTimeStampInTable(table)).Should().BeTrue();
    }

    public async Task StoreSensorData(Table table)
    {
        foreach (var row in table.Rows)
            await PutSensorData(row[SensorTableHeaders.TimeStamp], SensorDataAbstractFactory.Create(row));
    }
    public async Task StoreSensorDataPosX(Table table)
    {
        foreach (var row in table.Rows)
            await PutSensorData(row[SensorTableHeaders.TimeStamp], SensorDataAbstractFactory.CreatePosX(row));
    }

    public async Task StoreSensorDataPosY(Table table)
    {
        foreach (var row in table.Rows)
            await PutSensorData(row[SensorTableHeaders.TimeStamp], SensorDataAbstractFactory.CreatePosY(row));
    }
    public async Task<DashboardReportViewModel> FetchSensorData(long from, long to, string human)
    {
        var withQueryParams = ApiEntryPointUrl.OfSensorEventsReport
            .WithQueryParams(("from", from.ToString()), ("to", to.ToString()), ("filter", human));

        var httpResponseMessage = await _httpClient.GetAsync(withQueryParams);

        httpResponseMessage.EnsureSuccess();

        return httpResponseMessage.To<DashboardReportViewModel>();
    }

    public async Task Assert(DashboardReportViewModel real, Table table)
    {
        DashboardReportViewModel expected = SensorDataAbstractFactory.CreateDashboardReportViewModel(table.Rows);

        expected.Items.Count.Should().Be(real.Items.Count);
        expected.Items.
            SequenceEqual(real.Items).Should().BeTrue();
    }

    public async Task AssertPositionXSeries(DashboardReportViewModel real, Table table)
    {
        DashboardReportViewModel expected = SensorDataAbstractFactory.CreateDashboardReportViewModelPosX(table.Rows);

        expected.Items.Count.Should().Be(real.Items.Count);
        expected.Items.
            SequenceEqual(real.Items).Should().BeTrue();
    }

    public async Task AssertPositionYSeries(DashboardReportViewModel real, Table table)
    {
        DashboardReportViewModel expected = SensorDataAbstractFactory.CreateDashboardReportViewModelPosY(table.Rows);

        expected.Items.Count.Should().Be(real.Items.Count);
        expected.Items.
            SequenceEqual(real.Items).Should().BeTrue();
    }

    public async Task Assert(Table expectedEventTable)
    {
        var sensorDataViewModels = await GetSensorDataViewModels();
        sensorDataViewModels.Should().NotBeNull();
        var sensorDataViewModel = sensorDataViewModels.Single();

        AssertEqual(sensorDataViewModel, SensorDataAbstractFactory.Create(expectedEventTable));
    }
    
    private async Task PutSensorData(string timestamp, StoreSensorDataCommand command)
    {
        var result = await _httpClient.PutAsync(ApiEntryPointUrl.OfSensorEventsAt + timestamp, command.ToStringContent());
        result.EnsureSuccess();
    }

    private async Task<List<SensorDataViewModel>> GetSensorDataViewModels()
    {
        var result = await _httpClient.GetAsync(ApiEntryPointUrl.OfSensorEvents.WithQueryParams(("from", "0"), ("to", long.MaxValue.ToString())));
        result.EnsureSuccess();

        var sensorDataViewModels = result.To<SensorApiDataViewModel>();
        return sensorDataViewModels.SensorData;
    }


    private void AssertEqual(SensorDataViewModel sensorDataViewModels, SensorDataViewModel viewModel)
    {
        sensorDataViewModels.TimeStamp.Should().BeEquivalentTo(viewModel.TimeStamp);
        var a = sensorDataViewModels.Instances.Single();

        a.PositionX.Should().Be(viewModel.Instances.Single().PositionX);
        a.PositionY.Should().Be(viewModel.Instances.Single().PositionY);
        a.VelX.Should().Be(viewModel.Instances.Single().VelX);
        a.VelY.Should().Be(viewModel.Instances.Single().VelY);
    }
    
    private async Task<IEnumerable<long>> FetchListOfTimeStampOfStoredSensorEvents()
    {
        var result = await GetSensorDataViewModels();

        return result.Select(a => long.Parse(a.TimeStamp));
    }

    private static List<long> ListOfTimeStampInTable(Table table)
    {
        return new List<long>
        {
            long.Parse(table.Get(0, SensorTableHeaders.TimeStamp)),
            long.Parse(table.Get(1, SensorTableHeaders.TimeStamp)),
        };
    }
}