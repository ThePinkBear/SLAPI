using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class BadgeCreateRequest
{
  [Required]
  public string BadgeName { get; set; } = default!;
  [Required]
  public string PersonPrimaryId { get; set; } = default!;
}
