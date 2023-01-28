namespace FsTask.Domain.Contract;

public class StoreSensorDataCommandDto
{
    public string Id { get; set; }
    public long Timestamp { get; set; }
    public List<InstanceDto> Instances { get; set; }
}

public class InstanceDto
{
    public string HumanId { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public int VelX { get; set; }
    public int VelY { get; set; }
}