namespace SLAPI.Models;

public class ExosAccessControl
{
  [JsonPropertyName("PinCode")]
  public string? PinCode { get; set; } = default!;
  [JsonPropertyName("AccessRights")]
  public List<AccessRight>? accessRights { get; set; } = default!;
}