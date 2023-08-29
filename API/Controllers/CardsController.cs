using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Test.Models;

namespace SLAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CardsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _cardUrl;

    public CardsController(IHttpClientFactory client, IConfiguration config)
    {
      _cardUrl = config.GetValue<string>("Url:Card");
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<List<Badge>>> GetCard(string id)
    {
      var response = await _client.GetAsync($"{_url}{_cardUrl}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var card = JsonConvert.DeserializeObject<List<Badge>>(objectResult["value"]!
        .ToString())?
        .FirstOrDefault(x => x.BadgeIdInternal == id);

      return card == null ? NotFound() : Ok(card);
    }

    [HttpPost]
    public async Task<ActionResult<Badge>> CreateCard(BadgeCreateRequest badge)
    {

      // TODO What Id is created by exos and can that be retrieved from return of created or is a separate Get call needed and if so, with what searchparameter?
      var newBadge = new Badge
      {
        BadgeId = Guid.NewGuid().ToString(),
        BadgeName = badge.BadgeName,
        PersonPrimaryId = badge.PersonPrimaryId
      };
      var createdBadge = await _client.PostAsJsonAsync($"{_url}/v1.0/badges/create", newBadge);

      return Ok(createdBadge);

      // return CreatedAtAction("GetCard", new { id = newBadge.PersonPrimaryId }, createdBadge);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(string id)
    {
      _ = await _client.DeleteAsync($"{_url}/v1.0/badges/{id}/delete");
      return NoContent();
    }
  }
}
