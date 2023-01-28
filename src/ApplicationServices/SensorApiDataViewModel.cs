namespace FsTask.ApplicationServices;

public class SensorApiDataViewModel
{
    public List<SensorDataViewModel> SensorData { get; set; }=new();
    public object _links { get; set; }
}