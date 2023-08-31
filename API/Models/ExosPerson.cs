using Newtonsoft.Json;
using Test.Models;

public class ExosPerson
{
  [JsonProperty("PersonCategory")]
  public int PersonCategory { get; set; }
  [JsonProperty("PersonBaseData")]
  public Person PersonBaseData { get; set; } = default!;
}