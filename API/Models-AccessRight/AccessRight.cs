using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Test.Models;

public class AccessRight
{

    [JsonProperty("DisplayName")]
    public string BadgeName { get; set; } = default!;
    [Key]
    [JsonProperty("TenantId")]
    public string PersonPrimaryId { get; set; } = default!;
    [JsonProperty("AccessRightId")]
    public string BadgeId { get; set; } = default!;
    [JsonProperty("Comment")]
    public string TimeZoneIdInternal { get; set; } = default!; // Relationship with Schedule?
}