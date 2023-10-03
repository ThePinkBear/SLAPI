namespace Test.Models;

public class BetsyPersonPutRequest
{
    public string? PersonalNumber { get; set; } = default!;
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? PinCode { get; set; } = default!;
    public string? Department { get; set; } = default!;
}
