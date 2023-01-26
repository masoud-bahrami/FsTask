using FsTask.Domain.Contracts;

namespace FsTask.ApplicationServices;

public interface IStoreSensorEventService
{
    Task Process(StoreSensorDataCommand cmd, string at);
    Task<SensorApiDataViewModel> Get(string from, string to);

    Task<SensorDataViewModel> Get(string from);
}

public class StoreSensorEventService : IStoreSensorEventService
{
    static readonly Dictionary<string, StoreSensorDataCommand> Dictionary;

    static StoreSensorEventService()
    {
        Dictionary = new();
    }
    public Task Process(StoreSensorDataCommand cmd, string at)
    {
        Dictionary.Add(at, cmd);
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

    public async Task<SensorApiDataViewModel> Get(string from, string to )
    {
        return new SensorApiDataViewModel
        {
            SensorData =

            Dictionary
                .OrderBy(a => a.Key)
                .Select(a => ToSensorDataViewModel(a.Key, a.Value)).ToList(),
            _links = GenerateHyperLinks(from, to)
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
            _hasNext= true,
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

    private static SensorDataViewModel ToSensorDataViewModel(string timeStamp, StoreSensorDataCommand a)
    {
        return new SensorDataViewModel
        {
            TimeStamp = timeStamp,
            PositionX = a.PositionX,
            PositionY = a.PositionY,
            VelY = a.VelX,
            VelX = a.VelY,
            HumanId = a.HumanId,
        };
    }

    
}