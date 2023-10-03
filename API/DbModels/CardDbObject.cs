namespace SLDB.Models;

public class CardDbObject
{
  [Key, Required]
  public string CardNumber { get; set; } = default!;
  public string PersonPrimaryId { get; set; } = default!;
}
