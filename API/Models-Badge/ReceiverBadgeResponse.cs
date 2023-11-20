namespace SLAPI.Models;

public class ReceiverBadgeResponse
{
  public string CardNumber { get; set; } = default!;
  public string PersonPrimaryId { get; set; } = default!;
  public bool IsReleased { get; internal set; }
  public string Origin { get; set; } = default!;
  public string ValidFrom { get; set; } = default!;
  public string ValidTo { get; set; } = default!;
  public string LastModified { get; set; } = default!;
}
