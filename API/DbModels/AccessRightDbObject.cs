namespace SLDB.Models;

public class AccessRightDbObject
{
  [Key]
  public int AccessRightId { get; set; }
  public string UniqueId { get; set; } = default!;
  public string PersonPrimaryId { get; set; } = default!;
  public string AccessPointId { get; set; } = default!;
  public string ScheduleId { get; set; } = default!;
  public DateTime? ValidFrom { get; set; }
  public DateTime? ValidTo { get; set; }
}
