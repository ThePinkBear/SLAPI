namespace SLAPI.Models;

public class ReceiverPersonPutRequest
{
  [JsonPropertyName("PrimaryId")]
  public string PrimaryId { get; set; } = default!;
  [JsonPropertyName("FirstName")]
  public string? FirstName { get; set; } = default!;
  [JsonPropertyName("LastName")]
  public string? LastName { get; set; } = default!;
  [JsonPropertyName("PersonNumber")]
  public string? PersonNumber { get; set; }
  [JsonPropertyName("Phone")]
  public string? Phone { get; set; }
  [JsonPropertyName("PersonType")]
  public string? PersonType { get; set; }
  [JsonPropertyName("Company")]
  public string? Company { get; set; }
  [JsonPropertyName("Department")]
  public string? Department { get; set; }
  [JsonPropertyName("PinCode")]
  public string? PinCode { get; set; }
}
