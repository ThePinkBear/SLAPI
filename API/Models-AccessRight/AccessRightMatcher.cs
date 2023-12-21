namespace SLAPI.Models;

public class AccessRightMatcher
{
  [Key]
  public int Id { get; set; } = default!;
  public string rid { get; set; } = default!;
  public string aid { get; set; } = default!;
  public string sid { get; set; } = default!;
}
