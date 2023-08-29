using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class ScheduleDbObject
{
  [Key]
  public string ScheduleId { get; set; } = default!;
  public string Description { get; set; } = default!;
}
