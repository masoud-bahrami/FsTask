using FsTask.Domain.Contract;
using FsTask.QuestDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTask.IntegrationTests;

public class FilteredDataHumanTests
{
    private readonly SensorEventsToGuestDbService _sensorEventsToGuestDbService;

    public FilteredDataHumanTests()
    {
        _sensorEventsToGuestDbService = new SensorEventsToGuestDbService(new QuestDbConfig
        {
            Host = "localhost",
            Port = 9009,
            UserName = "admin",
            Password = "quest",
            DataBase = "qdb",
            NpsqlPort = 8812
        });
    }

    [Fact]
    public async Task testImportSensorDataToQuestDb()
    {


        var serializer = new JsonSerializer();
        await using FileStream s = File.Open(@"./FilteredDataHuman", FileMode.Open);
        using StreamReader sr = new StreamReader(s);
        using JsonReader reader = new JsonTextReader(sr);
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var filteredDataHumanItem = serializer.Deserialize<FilteredDataHumanItem>(reader);

                var storeSensorDataCommandDto = NewMethod(filteredDataHumanItem);
                await _sensorEventsToGuestDbService.Save(new StoreSensorDataCommand
                {
                    Instances = storeSensorDataCommandDto.Instances
                }, storeSensorDataCommandDto.Timestamp.ToString());
            }
        }
    }

    private static StoreSensorDataCommandDto NewMethod(FilteredDataHumanItem filteredDataHumanItem)
    {
        StoreSensorDataCommandDto result = new StoreSensorDataCommandDto
        {
            Id = filteredDataHumanItem.Id.oid,
            Timestamp = filteredDataHumanItem.TimeStamp.date.numberLong
        };

        List<InstanceDto> instances = new List<InstanceDto>();

        var instancesFirst = filteredDataHumanItem.Instances.First;

        do
        {
            instances.Add(InstanceDto(instancesFirst));

            instancesFirst = instancesFirst.Next;
        } while (instancesFirst != null);

        result.Instances = instances;

        return result;
    }

    private static InstanceDto InstanceDto(JToken instancesFirst)
    {
        InstanceDto dto = new InstanceDto();
        foreach (var jToken in instancesFirst.Children())
        {
            var positionX = jToken["pos_x"].Value<double>();
            var positionY = jToken["pos_y"].Value<double>();
            var velX = jToken["vel_x"].Value<int>();
            var velY = jToken["vel_y"].Value<int>();

            dto = new InstanceDto
            {
                HumanId = instancesFirst.Path,
                PositionX = positionX,
                PositionY = positionY,
                VelX = velX,
                VelY = velY
            };
        }

        return dto;
    }
}