using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Test.Models;

public class ExosBadgeResponse
{
    [JsonPropertyName("BadgeName")]
    public string BadgeName { get; set; } = default!;

    [JsonPropertyName("Person")]
    public Person Person { get; set; } = default!;
}