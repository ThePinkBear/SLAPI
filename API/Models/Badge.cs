using Newtonsoft.Json;

namespace Test.Models;

public class Badge
{
    [JsonProperty("BadgeName")]
    public string BadgeName { get; set; } = default!;
    // public string PersonPrimaryId { get; set; } = default!;
    [JsonProperty("BadgeIdInternal")]
    public string BadgeId { get; set; } = default!;
}