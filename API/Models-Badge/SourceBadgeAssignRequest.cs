namespace SLAPI.Models;

public class SourceBadgeAssignRequest
{
  [JsonPropertyName("BadgeName")]
  public string BadgeName { get; set; } = default!;
}