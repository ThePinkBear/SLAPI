namespace SLAPI.Models;

public class ExosPersonAccessRightResponse
{
  [JsonPropertyName("AssignmentId")]
  public string AssignmentId { get; set; } = default!;
  [JsonPropertyName("AccessRightId1")]
  public string AccessRightId { get; set; } = default!;
  [JsonPropertyName("DisplayName")]
  public string DisplayName { get; set; } = default!;
  [JsonPropertyName("TimeZoneId1")]
  public string TimeZoneId { get; set; } = default!;
  [JsonPropertyName("Comment")]
  public string Comment { get; set; } = default!;
  [JsonPropertyName("AccessRight")]
  public ExosAccessRightResponse AccessRight { get; set; } = default!;
  [JsonPropertyName("TimeZone")]
  public ExosScheduleResponse TimeZone { get; set; } = default!;


}