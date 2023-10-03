namespace SLAPI.Models;

public class AccessRight
{
  [JsonPropertyName("DisplayName")]
  public string BadgeName { get; set; } = default!;
  [JsonPropertyName("TenantId")]
  public string PersonPrimaryId { get; set; } = default!;
  [JsonPropertyName("AccessRightId")]
  public string BadgeId { get; set; } = default!;
  [JsonPropertyName("Comment")]
  public string TimeZoneIdInternal { get; set; } = default!;
}