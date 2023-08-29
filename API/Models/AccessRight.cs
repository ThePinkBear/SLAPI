namespace Test.Models;

public class AccessRight
{
    public string BadgeName { get; set; } = default!;
    public string PersonPrimaryId { get; set; } = default!;
    public string BadgeId { get; set; } = default!;
    public string TimeZoneIdInternal { get; set; } = default!; // Relationship with Schedule?
}