namespace SLAPI.Models;

public class ReceiverAccessRightRequest
{
  public string BadgeName { get; set; } = default!;
  public string PersonPrimaryId { get; set; } = default!;
  public string TimeZoneId { get; set; } = default!;
  public string AccessPointId { get; set; } = default!;
}
