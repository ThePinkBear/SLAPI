using NuGet.Protocol.Plugins;

namespace SLAPI.Models;

public class ExosUnassignmentRequest
{
  [JsonPropertyName("AssignmentId")]
  public string AssignMentId { get; set; } = default!;
  [JsonPropertyName("PersonId")]
  public string PersonId { get; set; } = default!;
}