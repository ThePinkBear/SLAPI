namespace Test.Models;

public class Person
{
  public string FullName { get => $"{LastName}, {FirstName}"; }
  public string PersonId { get; set; } = default!;
  public string PrimaryId { get; set; } = default!;
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public string Department { get; set; } = default!;
  public string PinCode { get; set; } = default!;
}
