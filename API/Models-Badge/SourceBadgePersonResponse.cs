namespace SLAPI.Models;

public class SourceBadgePersonResponse
{
  [JsonPropertyName("PersonBaseData")]
  public BadgePerson PersonBaseData { get; set; } = default!;
}