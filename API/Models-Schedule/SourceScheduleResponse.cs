namespace SLAPI.Models;

public class SourceScheduleResponse
{
    [JsonPropertyName("TimeZoneId")]
    public string TimeZoneId { get; set; } = default!;
    [JsonPropertyName("DisplayName")]
    public string DisplayName { get; set; } = default!;
    [JsonPropertyName("Comment")]
    public string Comment { get; set; } = default!;
}
