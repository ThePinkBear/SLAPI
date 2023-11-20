namespace SLAPI.Models;

public class ReceiverPersonPutRequest
{
  public string PersonalNumber { get; set; } = default!;
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public string PinCode { get; set; } = default!;
  public string Department { get; set; } = default!;
  public string PhoneNumber { get; set; } = default!;
}
