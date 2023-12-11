namespace SLAPI.Models;

public class ReceiverBadgeRequest
{
  [Required]
  [JsonPropertyName("CardNumber")]
  public string CardNumber { get; set; } = default!;
  [Required]  
  [JsonPropertyName("PersonPrimaryId")]
  public string PersonPrimaryId { get; set; } = default!;
}
