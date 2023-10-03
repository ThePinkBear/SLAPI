namespace SLAPI.Models;

public class ExosPersonRequest
{
  [JsonPropertyName("PersonBaseData")]
  public PersonBaseData PersonBaseData { get; set; } = default!;
  [JsonPropertyName("PersonTenantFreeFields")]
  public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
}
