using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Test.Models;

public class BadgeRequest
{
  [Required]
  public string BadgeName { get; set; } = default!;
}
