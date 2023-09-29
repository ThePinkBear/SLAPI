namespace Test.Models;

public class ExosPersonRequest
{
  [JsonPropertyName("PersonBaseData")]
  public PersonRequest PersonBaseData { get; set; } = default!;
  // [JsonPropertyName("PersonTenantFreeFields")]
  // public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
}
