using FsTask.AcceptanceTests.Helpers;
using FsTask.API;
using FsTask.ApplicationServices;
using FsTask.Domain.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FsTask.AcceptanceTests.StepDefinitions.Drivers;

class SensorDataApiDriver : ISensorDataDriver
{
    private readonly HttpClient _httpClient;
    private const string applicationUrl = "api/sensor/events";
    public SensorDataApiDriver()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                });
            });

        _httpClient = application.CreateClient();
    }
    public async Task SendData(string timestamp, Table eventTable)
        => await PutSensorData(timestamp, Command(eventTable));


    public async Task SendData(string timestamp)
    {
        await PutSensorData(timestamp, Command());
    }

    public async Task AssertTheOrderOfEvents(Table table)
    {
        var result = await GetSensorDataViewModels();
        var a = result[0];
        var b = result[1];

        long.Parse(a.TimeStamp).Should().Be(long.Parse(table.Get(0, SensorTableHeaders.TimeStamp)));

        long.Parse(b.TimeStamp).Should().Be(long.Parse(table.Get(1, SensorTableHeaders.TimeStamp)));
    }

    public async Task Assert(Table expectedEventTable)
    {
        var sensorDataViewModels = await GetSensorDataViewModels();
        sensorDataViewModels.Should().NotBeNull();
        var sensorDataViewModel = sensorDataViewModels.Single();

        AssertEqual(sensorDataViewModel, Create(expectedEventTable));
    }


    private async Task PutSensorData(string timestamp, StoreSensorDataCommand command)
    {
        var result = await _httpClient.PutAsync($"{applicationUrl}/at/" + timestamp, command.ToStringContent());
        result.EnsureSuccess();
    }
    private async Task<List<SensorDataViewModel>> GetSensorDataViewModels()
    {
        await Task.Delay(500);
        var result = await _httpClient.GetAsync($"{applicationUrl}?from{0}?to{long.MaxValue}");
        result.EnsureSuccess();

        var sensorDataViewModels = result.To<SensorApiDataViewModel>();
        return sensorDataViewModels.SensorData;
    }

    private void AssertEqual(SensorDataViewModel sensorDataViewModels, SensorDataViewModel viewModel)
    {
        sensorDataViewModels.TimeStamp.Should().BeEquivalentTo(viewModel.TimeStamp);
        sensorDataViewModels.PositionX.Should().BeEquivalentTo(viewModel.PositionX);
        sensorDataViewModels.PositionY.Should().BeEquivalentTo(viewModel.PositionY);
        sensorDataViewModels.VelX.Should().BeEquivalentTo(viewModel.VelX);
        sensorDataViewModels.VelY.Should().BeEquivalentTo(viewModel.VelY);
    }

    private SensorDataViewModel Create(Table eventTable)
    {
        return new SensorDataViewModel
        {
            TimeStamp = eventTable.Get(0, SensorTableHeaders.TimeStamp),
            HumanId = eventTable.Get(0, SensorTableHeaders.HumanId),
            PositionX = eventTable.Get(0, SensorTableHeaders.PositionX),
            PositionY = eventTable.Get(0, SensorTableHeaders.PositionY),
            VelX = eventTable.Get(0, SensorTableHeaders.VelX),
            VelY = eventTable.Get(0, SensorTableHeaders.VelY),
        };
    }

    private static StoreSensorDataCommand Command(Table eventTable)
    {
        return new StoreSensorDataCommand
        {
            HumanId = eventTable.Get(0, SensorTableHeaders.HumanId),
            PositionX = eventTable.Get(0, SensorTableHeaders.PositionX),
            PositionY = eventTable.Get(0, SensorTableHeaders.PositionY),
            VelX = eventTable.Get(0, SensorTableHeaders.VelX),
            VelY = eventTable.Get(0, SensorTableHeaders.VelY),
        };
    }
    private static StoreSensorDataCommand Command()
    {
        return new StoreSensorDataCommand
        {
            HumanId = "21325",
            PositionX = "5.6",
            PositionY = "2.1",
            VelX = "5.6",
            VelY = "2.1",
        };
    }
}