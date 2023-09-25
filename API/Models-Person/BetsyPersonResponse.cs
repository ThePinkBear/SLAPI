using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Test.Models;

public class BetsyPersonResponse
{
  [JsonPropertyName("PersonId")]
  public string PersonId { get; set; } = default!;
  [JsonPropertyName("PersonalNumber")]
  [Key]
  public string PersonalNumber { get; set; } = default!;

  [JsonPropertyName("FirstName")]
  public string FirstName { get; set; } = default!;
  [JsonPropertyName("LastName")]
  public string LastName { get; set; } = default!;
  [JsonPropertyName("Department")]
  public string Department { get; set; } = default!;
  [JsonPropertyName("PinCode")]
  public string PinCode { get; set; } = default!;

}
