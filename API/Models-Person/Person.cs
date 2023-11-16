namespace SLAPI.Models;

public class Person
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
  [JsonPropertyName("")]
  public string PersonNumber { get; set; } = default!;
  [JsonPropertyName("PhoneNumber")]
  public string PhoneNumber { get; set; } = default!;
  [JsonPropertyName("Hierarchy")]
  public string Department { get; set; } = default!;
  [JsonPropertyName("PinCode")]
  public string PinCode { get; set; } = default!;
  public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
}