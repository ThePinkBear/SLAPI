using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class AccessRightCreateRequest
{
    [Required]
    public string BadgeName { get; set; } = default!;
    [Required]
    public string PersonPrimaryId { get; set; } = default!;
    [Required]
    public string BadgeId { get; set; } = default!;
}
