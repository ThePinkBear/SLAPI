using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Test.Models;

public class PersonTenantFreeFields
{
  [JsonPropertyName("Text1")]
  public string? PersonType { get; set; }
  [JsonPropertyName("Text2")]
  public string? Company { get; set; }
  [JsonPropertyName("Text3")]
  public string? Department { get; set; }
}