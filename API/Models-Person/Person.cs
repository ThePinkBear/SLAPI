using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Test.Models;

public class Person
{
  [Key]
  [JsonPropertyName("PersonId")]
  public string PersonId { get; set; } = default!;
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
  [JsonPropertyName("FirstName")]
  public string FirstName { get; set; } = default!;
  [JsonPropertyName("LastName")]
  public string LastName { get; set; } = default!;
  [JsonPropertyName("Department")]
  public string Department { get; set; } = default!;
  [JsonPropertyName("PinCode")]
  public string PinCode { get; set; } = default!;
  // public List<Badge>? Badges { get; set; }
  // public List<AccessRight>? AccessRights { get; set; }
  public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
}