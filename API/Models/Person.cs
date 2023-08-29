using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Test.Models;

public class Person
{
  [JsonProperty("PersonIdInternal")]
  [Key]
  public string PersonId { get; set; } = default!;
  [JsonProperty("TenantId")]
  public string PrimaryId { get; set; } = default!;
  [JsonProperty("FirstName")]
  public string FirstName { get; set; } = default!;
  [JsonProperty("LastName")]
  public string LastName { get; set; } = default!;
  [JsonProperty("DepartmentFk")]
  public string Department { get; set; } = default!;
  [JsonProperty("PinCode")]
  public string PinCode { get; set; } = default!;
  public List<Badge>? Badges { get; set; }
  public List<AccessRight>? AccessRights { get; set; }
}
