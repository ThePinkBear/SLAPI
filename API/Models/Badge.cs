using Newtonsoft.Json;

namespace Test.Models;

public class Badge
{
    public string BadgeId { get; set; } = default!;
    [JsonProperty("BadgeName")]
    public string BadgeName { get; set; } = default!;
    [JsonProperty("Person:PersonalNumber")]
    public string PersonPrimaryId { get; set; } = default!;
    [JsonProperty("BadgeIdInternal")]
    public string BadgeIdInternal { get; set; } = default!;
}