namespace SLDB.Models;

public class PersonDbObject
{
  [Key, Required]
  public string PrimaryId { get; set; } = default!;
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public string Department { get; set; } = default!;
  public string PinCode { get; set; } = default!;
}
