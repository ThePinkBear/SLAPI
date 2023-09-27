using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Test.Models;

public class BetsyScheduleResponse
{
    [JsonPropertyName("ScheduleId")]
    public string ScheduleId { get; set; } = default!;
    [JsonPropertyName("Description")]
    public string Description { get; set; } = default!;
}
