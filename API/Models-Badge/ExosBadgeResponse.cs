namespace SLAPI.Models;

public class ExosBadgeResponse
{
  [JsonPropertyName("BadgeName")]
  public string BadgeName { get; set; } = default!;
  [JsonPropertyName("BadgeId")]
  public string BadgeId { get; set; } = default!;

  [JsonPropertyName("Person")]
  public ExosBadgePersonResponse Person { get; set; } = default!;
}