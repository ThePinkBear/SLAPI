namespace SLAPI.Models;

public class ExosScheduleResponse
{
    [Key]
    [JsonProperty("TimeZoneIdInternal")]
    public int TimeZoneIdInternal { get; set; }
    [JsonProperty("TimeZoneId")]
    public string TimeZoneId { get; set; } = default!;
    [JsonProperty("DisplayName")]
    public string DisplayName { get; set; } = default!;
}
