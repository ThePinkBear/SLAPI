namespace SLAPI.Models;

public class PersonBaseData
{
  [JsonPropertyName("LastName")]
  public string LastName { get; set; } = default!;
  [JsonPropertyName("FirstName")]
  public string FirstName { get; set; } = default!;
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
}