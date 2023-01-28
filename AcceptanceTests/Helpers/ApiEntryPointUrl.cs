namespace FsTask.AcceptanceTests.Helpers;

public class ApiEntryPointUrl
{
    public const string OfSensorEvents = "api/sensor/events";
    public const string OfSensorEventsReport = $"{OfSensorEvents}/report";
    public const string OfSensorEventsAt = $"{OfSensorEvents}/at/";
}

public static class ApiEntryPointUrlExtensions
{


    public static string WithQueryParams(this string url, params (string, string)[] parameters)
    {
        var isTheFirstQueryParam = true;
        var result = url;
        foreach (var parameter in parameters)
        {
            var separator = isTheFirstQueryParam ? "?" : "&";
            result += $"{separator}{parameter.Item1}={parameter.Item2}";
            isTheFirstQueryParam = false;
        }

        return result;
    }

}

