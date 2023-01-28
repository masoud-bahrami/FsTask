using Newtonsoft.Json;

namespace FsTask.IntegrationTests;

public class Date
{
    [JsonProperty("$numberLong")]
    public long numberLong { get; set; }
}