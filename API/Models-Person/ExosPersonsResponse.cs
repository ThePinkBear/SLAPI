namespace SLAPI.Models;

public class ExosPersonsResponse
{
  [JsonPropertyName("PersonBaseData")]
  public ExosPerson PersonBaseData { get; set; } = default!;
  [JsonPropertyName("PersonTenantFreeFields")]
  public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
  [JsonPropertyName("PersonAccessControlData")]
  public ExosAccessControl PersonAccessControlData { get; set; } = default!;
}