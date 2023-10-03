namespace SLAPI.Models;

public class ApplicationDefinition
{
  [JsonPropertyName("BadgeNumber")]
  public string BadgeNumber { get; set; } = default!;
  [JsonPropertyName("ApplicationDefinitionFk")]
  public int ApplicationDefinitionFk { get; set; } = 1;
  
}