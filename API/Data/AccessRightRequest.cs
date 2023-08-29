using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class AccessRightRequest
{
    [Required]
    public string BadgeName { get; set; } = default!;
    [Required]
    public string PersonPrimaryId { get; set; } = default!;
    [Required]
    public string TimeZoneId { get; set; } = default!;
}
