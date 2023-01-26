namespace FsTask.Domain.Contracts;

public class StoreSensorDataCommand
{
    public string HumanId { get; set; }
    public string PositionX { get; set; }
    public string PositionY { get; set; }
    public string VelX { get; set; }
    public string VelY { get; set; }
}