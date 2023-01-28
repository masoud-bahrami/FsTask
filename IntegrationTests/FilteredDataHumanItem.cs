using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FsTask.IntegrationTests;

public class FilteredDataHumanItem
{
    public TimeStamp TimeStamp { get; set; }
    [JsonProperty("_id")]
    public Id Id { get; set; }
    [JsonProperty("instances")]
    public JObject Instances { get; set; }
}