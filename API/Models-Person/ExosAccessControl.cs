namespace SLAPI.Models;

public class ExosAccessControl
{
  [JsonPropertyName("CurrentCardValidity")]
  public string IsEnabled { get; set; } = default!;
  [JsonPropertyName("AccessRights")]
  public List<ExosPersonAccessRightResponse>? accessRights { get; set; } = default!;
}