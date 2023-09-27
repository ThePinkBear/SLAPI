using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Test.Models;

public class BetsyPersonRequest
{
    [Required]
    public string PersonalNumber { get; set; } = default!;
    [Required]
    public string FirstName { get; set; } = default!;
    [Required]
    public string LastName { get; set; } = default!;
    public string Department { get; set; } = default!;
    public string PinCode { get; set; } = default!;
}
