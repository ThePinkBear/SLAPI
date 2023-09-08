using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Test.Models;

public class PersonResponse
{
  [JsonProperty("PersonalNumber")]
  [Key]
  public string PersonalNumber { get; set; } = default!;

  [JsonProperty("FirstName")]
  public string FirstName { get; set; } = default!;
  [JsonProperty("LastName")]
  public string LastName { get; set; } = default!;
  [JsonProperty("DepartmentFk")]
  public string Department { get; set; } = default!;
  [JsonProperty("PinCode")]
  public string PinCode { get; set; } = default!;
}
