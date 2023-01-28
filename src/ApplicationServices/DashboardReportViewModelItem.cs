namespace FsTask.ApplicationServices;

public class DashboardReportViewModelItem
{
    public long Timestamp { get; set; }
    public List<string> YAxisDatas { get; set; } = new();

    public override bool Equals(object? obj)
    {
        var that = (DashboardReportViewModelItem)obj;
        return this.Timestamp == that.Timestamp
               && that.YAxisDatas.SequenceEqual(that.YAxisDatas);
    }
}