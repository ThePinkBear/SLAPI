using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Test.Models;

internal class BadgeDeleteRequest
  {
    public string BadgeName { get; set; } = default!;
  }
