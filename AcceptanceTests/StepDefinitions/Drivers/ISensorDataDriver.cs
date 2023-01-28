using FsTask.ApplicationServices;

namespace FsTask.AcceptanceTests.StepDefinitions.Drivers;

internal interface ISensorDataDriver
{
    Task SendData(string timestamp, Table eventTable);
    Task Assert(Table expectedEventTable);
    Task SendData(string timestamp);
    Task AssertTheOrderOfEvents(Table table);
    Task StoreSensorData(Table table);
    Task StoreSensorDataPosX(Table table);
    Task StoreSensorDataPosY(Table table);

    Task<DashboardReportViewModel> FetchSensorData(long from, long to, string human);
    Task Assert(DashboardReportViewModel real, Table table);
    Task AssertPositionXSeries(DashboardReportViewModel result, Table table);
    Task AssertPositionYSeries(DashboardReportViewModel result, Table table);
}