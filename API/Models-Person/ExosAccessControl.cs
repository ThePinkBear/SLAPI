public class ExosAccessControl
{
  [JsonPropertyName("PinCode")]
  public string? PinCode { get; set; } = default!;
  public List<AccessRight>? accessRights { get; set; } = default!;
}