using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Test.Models;

public class AccessPoint
{
  [Key]
  [JsonProperty("Id")]
  public string Id { get; set; } = default!;
  [JsonProperty("Name")]
  public string AccessPointId { get; set; } = default!;
  [JsonProperty("DeviceAddress")]
  public string Address { get; set; } = default!;
  public string Description { get; set; } = default!;
}
