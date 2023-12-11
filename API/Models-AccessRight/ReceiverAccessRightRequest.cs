namespace SLAPI.Models;

public class ReceiverAccessRightRequest
{
  [JsonPropertyName("PersonPrimaryId")]
  public string PersonPrimaryId { get; set; } = default!;
  [JsonPropertyName("ScheduleId")]
  public string ScheduleId { get; set; } = default!;
  [JsonPropertyName("AccessPointId")]
  public string AccessPointId { get; set; } = default!;
}
