using System.Text.Json.Serialization;

namespace Test.Models;

public class BetsyBadgeResponse
{
  [JsonPropertyName("CardNumber")]
  public string CardNumber { get; set; } = default!;
  [JsonPropertyName("PersonPrimaryId")]
  public string PersonPrimaryId { get; set; } = default!;
}
