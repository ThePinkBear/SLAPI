namespace Test.Models;

public class BetsyPersonCreateRequest
{
    [JsonPropertyName("PersonalNumber")]
    public string PersonalNumber { get; set; } = default!;
    [JsonPropertyName("FirstName")]
    public string? FirstName { get; set; } = default!;
    [JsonPropertyName("LastName")]
    public string? LastName { get; set; } = default!;
    [JsonPropertyName("PinCode")]
    public string? PinCode { get; set; } = default!;
    [JsonPropertyName("Department")]
    public string? Department { get; set; } = default!;
}