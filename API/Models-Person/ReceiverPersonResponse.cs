namespace SLAPI.Models;

public class ReceiverPersonResponse
{
  [JsonPropertyName("FullName")]
  public string FullName { get; set; } = default!;
  [JsonPropertyName("PrimaryId")]
  public string PrimaryId { get; set; } = default!;
  [JsonPropertyName("FirstName")]
  public string? FirstName { get; set; }
  [JsonPropertyName("LastName")]
  public string? LastName { get; set; }
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
  [JsonPropertyName("IsEnabled")]
  public object? IsEnabled { get; set; }
  [JsonPropertyName("Origin")]
  public string? Origin { get; set; } = "A";
  [JsonPropertyName("LastModified")]
  public string? LastModified { get; set; }
}
