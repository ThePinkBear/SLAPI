namespace SLAPI.Models;

public class ExosAccessControl
{
  [JsonPropertyName("AccessRights")]
  public List<ExosPersonAccessRightResponse>? accessRights { get; set; } = default!;
}