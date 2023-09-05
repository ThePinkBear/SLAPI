using Newtonsoft.Json;
using Test.Models;

public class ExosPerson
{
  [JsonProperty("PersonBaseData")]
  public Person PersonBaseData { get; set; } = default!;
  [JsonProperty("PersonAccessControlData")]
  public ExosAccessControl PersonAccessControlData { get; set; } = default!;
}