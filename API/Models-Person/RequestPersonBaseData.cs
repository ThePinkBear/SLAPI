namespace SLAPI.Models;

public class RequestPersonBaseData
{
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
  [JsonPropertyName("FirstName")]
  public string FirstName { get; set; } = default!;
  [JsonPropertyName("LastName")]
  public string LastName { get; set; } = default!;
  // [JsonPropertyName("PersonNumber")]
  // public string PersonNumber { get; set; } = default!; // Might be broken
  [JsonPropertyName("PhoneNumber")]
  public string PhoneNumber { get; set; } = default!;
}