using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

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
  // [JsonProperty("DepartmentFk")]
  // public string Department { get; set; } = default!;
  // [JsonProperty("PinCode")]
  // public string PinCode { get; set; } = default!;
  // public List<Badge>? Badges { get; set; }
  // public List<AccessRight>? AccessRights { get; set; }
}
