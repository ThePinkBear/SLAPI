using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Test.Models;

public class PersonTenantFreeFields
{
  [JsonPropertyName("Text1")]
  public string Text1 { get; set; } = default!;
  [JsonPropertyName("Text2")]
  public string Text2 { get; set; } = default!;
  [JsonPropertyName("Text3")]
  public string Text3 { get; set; } = default!;
}