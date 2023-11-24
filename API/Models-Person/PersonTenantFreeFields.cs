namespace SLAPI.Models;
public class PersonTenantFreeFields
{
  [JsonPropertyName("Text1")]
  public string? Text1 { get; set; }
  [JsonPropertyName("Text2")]
  public string? Text2 { get; set; } // Faktiskt personnummer
  [JsonPropertyName("Text3")]
  public string? Text3 { get; set; } // ex: Svenskt Personnummer
}
  