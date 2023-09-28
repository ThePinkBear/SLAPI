using System.Text.Json.Serialization;

namespace Test.Models;

public class ExosBadgeDeleteRequest
{
  [JsonPropertyName("BadgeName")]
  public string BadgeName { get; set; } = default!;
}
