namespace FsTask.AcceptanceTests.Helpers;

public static class TableExtensions
{
    public static string Get(this Table table, int index, string header)
    {
        return table.Rows[index][header];
    }
    public static string Get(this Table table, string header)
    {
        return table.Get(0, header);
    }
}