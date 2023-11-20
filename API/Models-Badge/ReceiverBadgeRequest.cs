namespace SLAPI.Models;

public class ReceiverBadgeRequest
{
  [Required]
  [JsonPropertyName("CardNumber")]
  public string CardNumber { get; set; } = default!;
  [Required]  
  [JsonPropertyName("PersonalNumber")]
  public string PersonalNumber { get; set; } = default!;
}
