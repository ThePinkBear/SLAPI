namespace SLAPI.Models;

public class ExosBadgeAssignRequest
{
  [JsonPropertyName("BadgeName")]
  public string BadgeName { get; set; } = default!;
}