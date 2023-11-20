namespace SLAPI.Models;

public class SourceScheduleResponse
{
    [JsonProperty("TimeZoneId")]
    public string TimeZoneId { get; set; } = default!;
    [JsonProperty("DisplayName")]
    public string DisplayName { get; set; } = default!;
    [JsonProperty("Comment")]
    public string Comment { get; set; } = default!;
}
