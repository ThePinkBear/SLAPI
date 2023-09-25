namespace Test.Models;

public class BetsyAccessPointResponse
{
   
    public string AccessPointId { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Description { get; set; } = default!;// Needs same value as AccessPointId.
}
