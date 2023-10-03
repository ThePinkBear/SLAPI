namespace SLAPI.Models;

public class BetsyPersonResponse
{
  [JsonPropertyName("PersonId")]
  public string PersonId { get; set; } = default!;
  [JsonPropertyName("PrimaryId")]
  public string PersonalNumber { get; set; } = default!;
  [JsonPropertyName("FirstName")]
  public string? FirstName { get; set; }
  [JsonPropertyName("LastName")]
  public string? LastName { get; set; }
  [JsonPropertyName("Department")]
  public string? Department { get; set; }
}
