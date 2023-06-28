using Newtonsoft.Json;

namespace Test.Models;

public class AccessPoint
{
    [JsonProperty("AccessRightId")]
    public string AccessPointId { get; set; } = default!;
    [JsonProperty("Comment")]
    public string Address { get; set; } = default!;
    [JsonProperty("DisplayName")]
    public string Descripion { get; set; } = default!;
}
