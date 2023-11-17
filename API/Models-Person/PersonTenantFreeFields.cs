namespace SLAPI.Models;
public class PersonTenantFreeFields
{
  [JsonPropertyName("Text1")]
  public string? other { get; set; }
  [JsonPropertyName("Text2")]
  public string? PersonNumber { get; set; } // Faktiskt personnummer
  [JsonPropertyName("Text3")]
  public string? PersonType { get; set; } // ex: Svenskt Personnummer
}
  