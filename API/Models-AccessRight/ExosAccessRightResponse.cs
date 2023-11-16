namespace SLAPI.Models;

public class ExosAccessRightResponse
{
  [JsonPropertyName("AccessRightId")]
  public string AccessRightId { get; set; } = default!;
  [JsonPropertyName("AssignmentId")]
  public string AssignmentId { get; set; } = default!;
  [JsonPropertyName("DisplayName")]
  public string DisplayName { get; set; } = default!;
  [JsonPropertyName("Comment")]
  public string Comment { get; set; } = default!;
}