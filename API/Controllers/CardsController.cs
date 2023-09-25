using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Test.Models;
using static ByteContentService;

namespace SLAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CardsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _cardUrl1;
    private readonly string? _cardUrl2;
    private readonly ExosRepository _exosService;

    public CardsController(IHttpClientFactory client, IConfiguration config)
    {
      _cardUrl1 = config.GetValue<string>("Url:GetBadgeStart");
      _cardUrl2 = config.GetValue<string>("Url:GetBadgeEnd");
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _exosService = new ExosRepository();
    }

    [HttpGet]
    public async Task<ActionResult<List<BetsyBadgeResponse>>> GetCard(string? badgeName)
    {
      var cards = await _exosService.GetExos<Badge>(_client, $"{_url}{_cardUrl1}{badgeName}{_cardUrl2}", "value");

      var cardResponse = from card in cards
                         select new BetsyBadgeResponse
                         {
                           BadgeName = card!.BadgeName
                           //PersonPrimaryId = card.Person.PersonalNumber
                         };


      return Ok(cardResponse);
    }

    [HttpPost]
    public async Task<ActionResult<Badge>> CreateCard(BetsyBadgeRequest badge)
    {
      var newBadge = new ExosBadgeRequest
      {
        BadgeName = badge.BadgeName,
        MediaDefinitionFk = 1,
        ApplicationDefinitions = new List<ApplicationDefinition>
        {
          new ApplicationDefinition
          {
            BadgeNumber = badge.BadgeName,
            ApplicationDefinitionFk = 1
          }
        }
      };
      try
      {
        await _client.PostAsync($"{_url}/api/v1.0/badges/create", ByteMaker(newBadge));
        return NoContent();
      }
      catch (Exception)
      {
        return BadRequest();
      }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCard(string badgeName)
    {
      var deleteRequest = new BetsyBadgeRequest
      {
        BadgeName = badgeName
      };
      await _client.PostAsync($"{_url}/api/v1.0/badges/delete", ByteMaker(deleteRequest));
      return NoContent();
    }
  }
}
