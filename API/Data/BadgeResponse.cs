using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class BadgeResponse
{
  public string BadgeName { get; set; } = default!;
  public string PersonPrimaryId { get; set; } = default!;
}
