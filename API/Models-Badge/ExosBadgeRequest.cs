using System.Text.Json.Serialization;

namespace Test.Models;

public class ExosBadgeRequest
{
  [JsonPropertyName("BadgeName")]
  public string BadgeName { get; set; } = default!;
  [JsonPropertyName("MediaDefinitionFk")]
  public int MediaDefinitionFk { get; set; } = default!;
  [JsonPropertyName("MediaRoleAuthorisation")]
  public string MediaRoleAuthorisation { get; internal set; } = default!;
  [JsonPropertyName("ApplicationDefinitions")]
  public List<ApplicationDefinition> ApplicationDefinitions { get; set; } = default!;
  // [JsonPropertyName("Person")]
  // public ExosPerson Person { get; set; } = default!;
}
