namespace SLAPI.Models;

public class PersonBaseData
{
  [JsonPropertyName("FullName")]
  public string Fullname { get; set; } = default!;
  [JsonPropertyName("PersonId")]
  public string PersonId { get; set; } = default!;
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
  [JsonPropertyName("Hierarchy")]
  public string Hierarchy { get; set; } = default!;
  [JsonPropertyName("PinCode")]
  public string PinCode { get; set; } = default!;
  [JsonPropertyName("IsEnabled")]
  public bool IsEnabled { get; set; } = default!;
  [JsonPropertyName("LastChangeDate")]
  public string LastChangeDate { get; set; } = default!;
}