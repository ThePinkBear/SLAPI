using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Test.Models;

public class BetsyScheduleResponse
{
    [Key]
    [JsonProperty("TimeZoneIdInternal")]
    public int TimeZoneIdInternal { get; set; }
    [JsonProperty("TimeZoneId")]
    public string TimeZoneId { get; set; } = default!;
    [JsonProperty("DisplayName")]
    public string DisplayName { get; set; } = default!;
}
