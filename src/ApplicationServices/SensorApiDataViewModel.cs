namespace FsTask.ApplicationServices;

public class SensorApiDataViewModel
{
    public List<SensorDataViewModel> SensorData { get; init; }=new();
    public object _links { get; set; }
}