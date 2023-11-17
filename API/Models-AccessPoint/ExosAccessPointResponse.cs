namespace SLAPI.Models;

public class ExosAccessPointResponse
{
  [Key]
  [JsonProperty("Id")]
  public string Id { get; set; } = default!;
  [JsonProperty("Name")]
  public string AccessPointId { get; set; } = default!;
  [JsonProperty("DeviceAddress")]
  public string Address { get; set; } = default!;
}
