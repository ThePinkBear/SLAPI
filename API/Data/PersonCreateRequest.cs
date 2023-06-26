using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class PersonCreateRequest
{
    [Required]
    public string FirstName { get; set; } = default!;
    [Required]
    public string LastName { get; set; } = default!;
    [Required]
    public string Department { get; set; } = default!;
    [Required]
    public string PinCode { get; set; } = default!;
}
