namespace Test.Models;

public class PersonRequest
{
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
  [JsonPropertyName("FirstName")]
  public string FirstName { get; set; } = default!;
  [JsonPropertyName("LastName")]
  public string LastName { get; set; } = default!;
}