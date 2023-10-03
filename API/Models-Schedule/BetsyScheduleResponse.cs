namespace SLAPI.Models;

public class BetsyScheduleResponse
{
  [JsonPropertyName("ScheduleId")]
  public string ScheduleId { get; set; } = default!;
  [JsonPropertyName("Description")]
  public string Description { get; set; } = default!;
}
