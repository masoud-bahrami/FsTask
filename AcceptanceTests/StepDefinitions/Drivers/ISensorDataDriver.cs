using System.Runtime.InteropServices;
using FsTask.API.Controllers;

namespace FsTask.AcceptanceTests.StepDefinitions.Drivers;

internal interface ISensorDataDriver
{
    Task SendData(string timestamp, Table eventTable);
    Task Assert(Table expectedEventTable);
    Task SendData(string timestamp);
    Task AssertTheOrderOfEvents(Table table);
}