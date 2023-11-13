namespace SLAPI.Models;

public class BadgePerson
{
  [JsonPropertyName("PersonId")]
  public string PersonalNumber { get; set; } = default!;
}