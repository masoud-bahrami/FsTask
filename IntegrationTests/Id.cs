using Newtonsoft.Json;

namespace FsTask.IntegrationTests;

public class Id
{
    [JsonProperty("$oid")]
    public string oid { get; set; }
}