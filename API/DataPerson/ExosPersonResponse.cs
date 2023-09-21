using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Test.Models;

public class ExosPersonResponse
{
  [JsonPropertyName("PersonBaseData")]
  public Person PersonBaseData { get; set; } = default!;
  [JsonPropertyName("PersonAccessControlData")]
  public ExosAccessControl PersonAccessControlData { get; set; } = default!;
  [JsonPropertyName("PersonTenantFreeFields")]
  public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
}