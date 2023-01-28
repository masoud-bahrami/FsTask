using System.Net;
using FsTask.Domain.Contract;

namespace FsTask.Domain;

public class SensorReceivedEventsAggregate
{
    public string Id { get; set; }
    public long At { get; private set; }

    public DateTime AtDateTime()
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(At).DateTime;
    }
    public HumanEnvironmentalStatistics HumanEnvironmentalStatistics { get; init; }

    public SensorReceivedEventsAggregate(string id, long at, StoreSensorDataCommand dto)
    {
        Id = id;
        At = at;
        HumanEnvironmentalStatistics = new HumanEnvironmentalStatistics(dto.Instances);
    }

        
    public override bool Equals(object? obj)
    {
        var that = (SensorReceivedEventsAggregate)obj;
        return this.Id == that.Id;
    }
}