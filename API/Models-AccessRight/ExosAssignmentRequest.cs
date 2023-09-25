using System.Text.Json.Serialization;

namespace Test.Models;

public class ExosAssignmentRequest
{
    [JsonPropertyName("AccessRightId")]
    public string AccessRightId { get; set; } = default!;
    [JsonPropertyName("TimeZoneId")]
    public string TimeZoneId { get; set; } = default!;
}