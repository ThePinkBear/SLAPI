using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class AccessRightResponse
{

    public string BadgeId { get; set; } = default!;
    public string PersonPrimaryId { get; set; } = default!;
    public string TimeZoneId { get; set; } = default!;
}
