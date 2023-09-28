using System.Text.Json.Serialization;
using Test.Models;

public class ExosAccessControl
{
  [JsonPropertyName("PinCode")]
  public string? PinCode { get; set; } = default!;
  public List<AccessRight>? accessRights { get; set; } = default!;
}