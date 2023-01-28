using Newtonsoft.Json;

namespace FsTask.IntegrationTests;

public class TimeStamp
{
    [JsonProperty("$date")]
    public Date date { get; set; }
}