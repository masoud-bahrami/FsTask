namespace FsTask.Domain;

public class HumanEnvironmentalStatistic
{
    public string HumanId { get; private set; }
    public Position Position { get; private set; }
    public Velocity Velocity { get; private set; }

    public HumanEnvironmentalStatistic(string humanId, Position position, Velocity velocity)
    {
        HumanId = humanId;
        Position = position;
        Velocity = velocity;
    }

    
}