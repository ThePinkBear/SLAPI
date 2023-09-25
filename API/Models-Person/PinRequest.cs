using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Test.Models;

public class PinRequest
{
    [Required]
    public string PinCode { get; set; } = default!;
}
