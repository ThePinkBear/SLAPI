namespace SLAPI.Models;

public class BetsyAccessRightResponse
{
  public int Id { get; set; }
  public string AccessRightId { get; set; } = default!;
  public string PersonPrimaryId { get; set; } = default!;
  public string AccessPointId { get; set; } = default!;
  public string ScheduleId { get; set; } = default!;
  public DateTime ValidFrom { get; set; }
  public DateTime ValidTo { get; set; }
}
