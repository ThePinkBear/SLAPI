namespace SLAPI.Models;

public class ReceiverAccessPointResponse
{
  [JsonPropertyName("AccessPointId")]
  public string AccessPointId { get; set; } = default!;
  [JsonPropertyName("Address")]
  public string Address { get; set; } = default!;
  [JsonPropertyName("Description")]
  public string Description { get; set; } = default!;// Needs same value as AccessPointId.
  // [JsonPropertyName("LastModified")]
  // public string LastModified { get; set; } = default!;
}
