namespace Test.Models;

public class ExosBadgePersonResponse
{
  [JsonPropertyName("PersonBaseData")]
  public BadgePerson PersonBaseData { get; set; } = default!;
}