using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class AccessAssignment
{
    [Key]
    public int Id { get; set; }
    public string AccessPointId { get; set; } = default!;
    public string ScheduleId { get; set; } = default!;
}
