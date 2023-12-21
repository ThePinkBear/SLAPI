namespace SLAPI.Models;

public class SourceAssignmentRequest
{
  [JsonPropertyName("AccessRightId")]
  public string AccessRightId { get; set; } = default!;
  [JsonPropertyName("TimeZoneId")]
  public string TimeZoneId { get; set; } = default!;
  // [JsonPropertyName("Comment")]
  // public string Comment { get; set; } = DateTime.Now.ToString();
}