using Microsoft.AspNetCore.Mvc;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CardsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _cardUrl;
    private readonly string _credentials;

    public CardsController(IHttpClientFactory client, IConfiguration config)
    {
      var c = new Credentials(config);
      _cardUrl = config.GetValue<string>("Url:Card");
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _credentials = c.Value;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Badge>> GetCard(string id)
    {
      _client.DefaultRequestHeaders.Authorization = new System.Net.Http
              .Headers.AuthenticationHeaderValue("Basic", _credentials);
      //TODO: Ensure Class Badge is compatible with response from exos, otherwise see accesspointController for reference.
      var badges = await _client.GetFromJsonAsync<List<Badge>>($"{_url}{_cardUrl}");

      var badge = badges?.FirstOrDefault(x => x.BadgeId == id);

      return badge == null ? NotFound() : badge;
    }

    [HttpPost]
    public async Task<ActionResult<Badge>> CreateCard(BadgeCreateRequest badge)
    {
      _client.DefaultRequestHeaders.Authorization = new System.Net.Http
              .Headers.AuthenticationHeaderValue("Basic", _credentials);
      var newBadge = new Badge
      {
        BadgeId = Guid.NewGuid().ToString(),
        BadgeName = badge.BadgeName,
        PersonPrimaryId = badge.PersonPrimaryId
      };
      var createdBadge = await _client.PostAsJsonAsync($"{_url}/v1.0/badges/create", newBadge);

      return CreatedAtAction("GetCard", new { id = newBadge.PersonPrimaryId }, createdBadge);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(string id)
    {
      _client.DefaultRequestHeaders.Authorization = new System.Net.Http
              .Headers.AuthenticationHeaderValue("Basic", _credentials);
  
      _ = await _client.DeleteAsync($"{_url}/v1.0/badges/{id}/delete");
      return NoContent();
    }
  }
}
