namespace SLAPI.Models;

public class ReceiverPersonCreateRequest
{
  [Required]
  [JsonPropertyName("PrimaryId")]
  public string PrimaryId { get; set; } = default!; 
  [JsonPropertyName("FirstName")]
  public string? FirstName { get; set; } = default!;
  [JsonPropertyName("LastName")]
  public string? LastName { get; set; } = default!;
  [JsonPropertyName("PersonNumber")]
  public string? PersonNumber { get; set; } = default!;
  [JsonPropertyName("Phone")]
  public string? Phone { get; set; } = default!;
  [JsonPropertyName("PersonType")]
  public string? PersonType { get; set; } = default!;
  [JsonPropertyName("Company")]
  public string? Company { get; set; } = default!;
  [JsonPropertyName("Department")]
  public string? Department { get; set; } = default!;
  [JsonPropertyName("PinCode")]
  public string? PinCode { get; set; } = default!; 
  [JsonPropertyName("IsEnabled")]
  public bool IsEnabled { get; set; }
  [JsonPropertyName("Origin")]
  public string Origin { get; set; } = default!;

}
