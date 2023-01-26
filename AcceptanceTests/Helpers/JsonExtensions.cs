using Newtonsoft.Json;

namespace FsTask.AcceptanceTests.Helpers;

public static class JsonExtensions
{
    public static T Deserialize<T>(this string result)
    {
        return JsonConvert.DeserializeObject<T>(result);
    }

    public static string Serialize<T>(this T result)
    {
        return JsonConvert.SerializeObject(result);
    }
}