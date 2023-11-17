using NuGet.Protocol.Plugins;

namespace SLAPI.Models;

public class DbExosUnassignRequest
{
  [Key]
  public int Id { get; set;}
  public string AssignMentId { get; set; } = default!;
  public string PersonId { get; set; } = default!;
  public string AccessRightId { get; set; } = default!;
}