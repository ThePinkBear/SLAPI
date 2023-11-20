namespace SLAPI.Models;

public class SourceBadgeDeleteRequest
{
  [JsonPropertyName("BadgeName")]
  public string BadgeName { get; set; } = default!;
}
