using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Test.Models;

public class BetsyBadgeRequest
{
  [Required]
  [JsonPropertyName("CardNumber")]
  public string CardNumber { get; set; } = default!;
  [Required]  
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
}
