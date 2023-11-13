using NuGet.Protocol.Plugins;

namespace SLAPI.Models;

public class ExosUnassignRequest
{
  [Key]
  public int Id { get; set;}
  public string AssignMentId { get; set; } = default!;
  public string PersonId { get; set; } = default!;
}