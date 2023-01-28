using System.Collections;
using FsTask.Domain.Contract;
using FsTask.Domain.Exception;

namespace FsTask.Domain;

public class HumanEnvironmentalStatistics : IEnumerable<HumanEnvironmentalStatistic>
{
    private readonly ICollection<HumanEnvironmentalStatistic> _instances;
    public HumanEnvironmentalStatistics(List<InstanceDto> dtoInstances)
    {
        _instances = new List<HumanEnvironmentalStatistic>();

        foreach (var dtoInstance in dtoInstances)
        {
            Add(dtoInstance);
        }
    }

    private void Add(InstanceDto dtoInstance)
    {
        var instance = OfHuman(dtoInstance.HumanId);

        if (instance!= null && (instance.Position != Position.New(dtoInstance.PositionX, dtoInstance.PositionY)))
            throw new HumanBeingInDifferentPositionAtTheSameTimeException(dtoInstance.HumanId);

        // TODO if position is the same so why we should add it again?
        _instances.Add(CreateInstance(dtoInstance) ?? throw new ArgumentNullException("CreateInstance(dtoInstance)"));
    }

    private static HumanEnvironmentalStatistic CreateInstance(InstanceDto dtoInstance)
    {
        return new HumanEnvironmentalStatistic(dtoInstance.HumanId,
            Position.New(dtoInstance.PositionX, dtoInstance.PositionY),
            Velocity.New(dtoInstance.VelX, dtoInstance.VelY));
    }

    public IEnumerator<HumanEnvironmentalStatistic> GetEnumerator()
    {
        return _instances.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _instances.GetEnumerator();
    }


    public HumanEnvironmentalStatistic OfHuman(string humanId) 
        => _instances.FirstOrDefault(i => i.HumanId == humanId);

    public List<string> GetHumans()
    {
        return _instances
            .Select(s => s.HumanId).ToList();
    }

    public List<string> GetPositionXs()
    {
        return _instances
            .Select(s => s.Position.X.ToString()).ToList();
    }

    public List<string> GetPositionYs()
    {
        return _instances
            .Select(s => s.Position.Y.ToString()).ToList();
    }
}