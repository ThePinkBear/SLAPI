namespace SLAPI.Models;

public class SourcePersonResponse
{
  [JsonPropertyName("PersonBaseData")]
  public SourcePerson PersonBaseData { get; set; } = default!;
  [JsonPropertyName("PersonAccessControlData")]
  public SourceAccessControl PersonAccessControlData { get; set; } = default!;

  [JsonPropertyName("PersonTenantFreeFields")]
  public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
}