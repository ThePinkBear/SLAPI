namespace SLAPI.Models;

public class ExosBadgeResponse
{
  [JsonPropertyName("BadgeName")]
  public string BadgeName { get; set; } = default!;
  [JsonPropertyName("BadgeId")]
  public string BadgeId { get; set; } = default!;
  [JsonPropertyName("MediaUsageData")]
  public BadgeMediaUsageData MediaUsageData { get; set; } = default!;
  [JsonPropertyName("Person")]
  public ExosBadgePersonResponse Person { get; set; } = default!;
  [JsonPropertyName("LastChangeDate")]
  public string LastChangeDate { get; set; } = default!;
  public string ValidTo { get; set; } = default!;
  public string ValidFrom { get; set; } = default!;
}