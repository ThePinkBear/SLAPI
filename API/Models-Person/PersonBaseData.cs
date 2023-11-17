namespace SLAPI.Models;

public class PersonBaseData
{
  [JsonPropertyName("FullName")]
  public string Fullname { get; set; } = default!;
  [JsonPropertyName("LastName")]
  public string LastName { get; set; } = default!;
  [JsonPropertyName("FirstName")]
  public string FirstName { get; set; } = default!;
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
  [JsonPropertyName("PhoneNumber")]
  public string PhoneNumber { get; set; } = default!;
  [JsonPropertyName("Hierarchy")]
  public string Department { get; set; } = default!;
  [JsonPropertyName("PinCode")]
  public string PinCode { get; set; } = default!;
  [JsonPropertyName("LastChangeDate")]
  public string LastModified { get; set; } = default!;
}