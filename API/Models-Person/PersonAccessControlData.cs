namespace SLAPI.Models;

public class PersonAccessControlData
{
  public string? CurrentCardNumber { get; set; } = default!;
  public string? CurrentCardValidity { get; set; } = default!;
  public List<SourcePersonAccessRightResponse>? AccessRights { get; set; } = default!;
}