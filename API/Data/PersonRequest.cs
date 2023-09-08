using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Test.Models;

public class PersonRequest
{
    [Required]
    [JsonProperty("PersonalNumber")]
    public string PersonalNumber { get; set; } = default!;
    [Required]
    public string FirstName { get; set; } = default!;
    [Required]
    public string LastName { get; set; } = default!;
    // [Required]
    // public string Department { get; set; } = default!;
    // [Required]
    // public string PinCode { get; set; } = default!;
}
