using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Test.Models;

public class Badge
{
    [JsonPropertyName("BadgeName")]
    public string BadgeName { get; set; } = default!;
    [JsonPropertyName("MediaDefinitionFk")]
    public int MediaDefinitionFk { get; set; } = 1;
    [JsonPropertyName("MediaRoleAuthorisation")]
    public string MediaRoleAuthorisation { get; set; } = "All";
    
    [JsonPropertyName("ApplicationDefinitions")]
    public List<ApplicationDefinition> ApplicationDefinitions { get; set; } = default!;
    [JsonPropertyName("Person")]
    public Person Person { get; set; } = default!;
}