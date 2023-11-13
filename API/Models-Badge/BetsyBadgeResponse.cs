namespace SLAPI.Models;

public class BetsyBadgeResponse
{
  [JsonPropertyName("CardName")]
  public string CardName { get; set; } = default!;
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
}
