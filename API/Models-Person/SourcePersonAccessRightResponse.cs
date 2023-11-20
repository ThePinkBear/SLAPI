namespace SLAPI.Models;

public class SourcePersonAccessRightResponse
{
  [JsonPropertyName("AssignmentId")]
  public string AssignmentId { get; set; } = default!;
  [JsonPropertyName("AccessRightId1")]
  public string AccessRightId { get; set; } = default!;
  [JsonPropertyName("DisplayName")]
  public string DisplayName { get; set; } = default!;
  [JsonPropertyName("TimeZoneId1")]
  public string TimeZoneId { get; set; } = default!;
  [JsonPropertyName("AccessRight")]
  public SourceAccessRightResponse AccessRight { get; set; } = default!;
  [JsonPropertyName("TimeZone")]
  public SourceScheduleResponse TimeZone { get; set; } = default!;


}