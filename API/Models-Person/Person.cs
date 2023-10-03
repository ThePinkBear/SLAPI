namespace SLAPI.Models;

public class Person
{
  [JsonPropertyName("PersonId")]
  public string PersonId { get; set; } = default!;
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
  [JsonPropertyName("FirstName")]
  public string FirstName { get; set; } = default!;
  [JsonPropertyName("LastName")]
  public string LastName { get; set; } = default!;
  [JsonPropertyName("PinCode")]
  public string PinCode { get; set; } = default!;
  // public List<ExosBadgeResponse>? Badges { get; set; }
  // public List<AccessRight>? AccessRights { get; set; }
  public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
}