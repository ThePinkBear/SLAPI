namespace SLAPI.Models;

public class ExosBadgeDeleteRequest
{
  [JsonPropertyName("BadgeName")]
  public string BadgeName { get; set; } = default!;
}
