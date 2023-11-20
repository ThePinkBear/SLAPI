namespace SLAPI.Models;

public class SourceAccessControl
{
  [JsonPropertyName("CurrentCardValidity")]
  public string IsEnabled { get; set; } = default!;
  [JsonPropertyName("AccessRights")]
  public List<SourcePersonAccessRightResponse>? accessRights { get; set; } = default!;
}