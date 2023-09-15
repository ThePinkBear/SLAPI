using System.Text.Json.Serialization;

namespace Test.Models;

public class BadgeExosRequest
{
    [JsonPropertyName("BadgeName")]
    public string BadgeName { get; set; } = default!;
    [JsonPropertyName("MediaDefinitionFk")]
    public int MediaDefinitionFk { get; set; } = default!;
    [JsonPropertyName("MediaRoleAuthorisation")]
    public string MediaRoleAuthorisation { get; set; } = "All";
    
    [JsonPropertyName("ApplicationDefinitions")]
    public List<ApplicationDefinition> ApplicationDefinitions { get; set; } = default!;
}
