namespace Test.Models;

public class PersonResponse
{
  // public string FullName { get => $"{LastName}, {FirstName}"; }
  public string PersonId { get; set; } = default!;
  public string PrimaryId { get; set; } = default!;
  // public string FirstName { get; set; } = default!;
  // public string LastName { get; set; } = default!;
}
