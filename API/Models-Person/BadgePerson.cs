namespace SLAPI.Models;

public class BadgePerson
{
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
}