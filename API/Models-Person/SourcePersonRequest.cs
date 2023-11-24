namespace SLAPI.Models;

public class SourcePersonRequest
{
  [JsonPropertyName("PersonBaseData")]
  public RequestPersonBaseData PersonBaseData { get; set; } = default!;
  [JsonPropertyName("PersonTenantFreeFields")]
  public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
}
