namespace Test.Models;

public class Schedule
{
    public int TimeZoneIdInternal { get; set; }
    public string TimeZoneId { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}
