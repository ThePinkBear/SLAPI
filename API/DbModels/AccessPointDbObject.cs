using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class AccessPointDbObject
{
  [Key, Required]
  public string AccessPointId { get; set; } = default!;
  public string Address { get; set; } = default!;
  public string Description { get; set; } = default!;
}
