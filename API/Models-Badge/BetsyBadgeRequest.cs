using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Test.Models;

public class BetsyBadgeRequest
{
  [Required]
  public string CardNumber { get; set; } = default!;
}
