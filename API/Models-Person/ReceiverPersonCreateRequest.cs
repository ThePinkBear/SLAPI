namespace SLAPI.Models;

public class ReceiverPersonCreateRequest
{
  [Required]
  [JsonPropertyName("PersonalNumber")] // PRIMARYID in log
  public string PersonalNumber { get; set; } = default!; 
  [JsonPropertyName("FirstName")] // FIRSTNAME in log
  public string? FirstName { get; set; } = default!;
  [JsonPropertyName("LastName")] // LASTNAME in log
  public string? LastName { get; set; } = default!;
  [JsonPropertyName("PinCode")] // PINCODE in log
  public string? PinCode { get; set; } = default!; 
  // [JsonPropertyName("Department")] // DEPARTMENT in log
  // public string? Department { get; set; } = default!;
  [JsonPropertyName("PhoneNumber")]
  public string? PhoneNumber { get; set; } = default!;
  [JsonPropertyName("PersonType")]
  public string? PersonType { get; set; } = default!;
  [JsonPropertyName("Company")]
  public string? Company { get; set; } = default!;
  [JsonPropertyName("PersonNumber")]
  public string? PersonNumber { get; set; } = default!;
  // [JsonPropertyName("IsEnabled")]
  // public int IsEnabled { get; set; } //Confirmed int from Betsy 1 is enabled
}
