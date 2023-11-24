namespace SLAPI.Models;

public class Value
{
  public PersonBaseData PersonBaseData { get; set; } = default!;
  public PersonAccessControlData PersonAccessControlData { get; set; } = default!;
  public PersonTenantFreeFields PersonTenantFreeFields { get; set; } = default!;
}