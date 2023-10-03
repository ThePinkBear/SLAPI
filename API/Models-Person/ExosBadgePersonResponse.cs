namespace SLAPI.Models;

public class ExosBadgePersonResponse
{
  [JsonPropertyName("PersonBaseData")]
  public BadgePerson PersonBaseData { get; set; } = default!;
}