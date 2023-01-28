namespace FsTask.AcceptanceTests.Helpers;

public class ApiEntryPointUrlExtensionsTests
{
    [Fact]
    public void testConcatingQueryParams()
    {
        ApiEntryPointUrl.OfSensorEvents
            .WithQueryParams(("from", "1"))
            .Should()
            .BeEquivalentTo(ApiEntryPointUrl.OfSensorEvents + "?from=1")
            ;

        ApiEntryPointUrl.OfSensorEvents
            .WithQueryParams(("from", "1"), ("to", "2"))
            .Should()
            .BeEquivalentTo(ApiEntryPointUrl.OfSensorEvents + "?from=1&to=2");
    }
}