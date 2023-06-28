// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Test.Models;

// namespace TestAPI.Controllers
// {
//   [Route("api/[controller]")]
//   [ApiController]
//   public class CardsController : ControllerBase
//   {
//     private readonly HttpClient _client;
//     private readonly string? _url;


//     public CardsController(HttpClient client, IConfiguration config)
//     {
//       _client = client;
//       _url = config.GetValue<string>("ExosUrl");
//     }

//     [HttpGet("{id}")]
//     public async Task<ActionResult<Badge>> GetCard(string id)
//     {
//       var badges = await _client.GetFromJsonAsync<List<Badge>>($"{_url}/v1.0/badges");

//       var badge = badges == null ? null : badges.FirstOrDefault(x => x.BadgeId == id);

//       return badge == null ? NotFound() : badge;
//     }

//     [HttpPost]
//     public async Task<ActionResult<Badge>> CreateCard(BadgeCreateRequest badge)
//     {
//       var newBadge = new Badge
//       {
//         BadgeId = Guid.NewGuid().ToString(),
//         BadgeName = badge.BadgeName,
//         PersonPrimaryId = badge.PersonPrimaryId
//       };
//       var createdBadge = await _client.PostAsJsonAsync($"{_url}/v1.0/badges/create", newBadge);

//       //TODO: Do some verification with createdBadge.

//       return CreatedAtAction("GetPerson", new { id = newBadge.PersonPrimaryId }, newBadge);
//     }

//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteCard(string id)
//     {
//       var deletedBadge = await _client.DeleteAsync($"{_url}/v1.0/badges/{id}/delete");
//       return NoContent();
//     }
//   }
// }
