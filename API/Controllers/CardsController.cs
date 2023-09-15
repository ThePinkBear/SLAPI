using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Test.Models;
using static ByteContent;

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

    public CardsController(IHttpClientFactory client, IConfiguration config)
    {
      _cardUrl1 = config.GetValue<string>("Url:GetBadgeStart");
      _cardUrl2 = config.GetValue<string>("Url:GetBadgeEnd");
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
    }

    [HttpGet]
    public async Task<ActionResult<List<BadgeResponse>>> GetCard(string? badgeName)
    {
      var response = await _client.GetAsync($"{_url}{_cardUrl1}{badgeName}{_cardUrl2}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var cards = JsonConvert.DeserializeObject<List<Badge>>(objectResult["value"]!.ToString());

      var cardResponse = from card in cards select new BadgeResponse
        {
          BadgeName = card!.BadgeName
          //PersonPrimaryId = card.Person.PersonalNumber
        };


      return Ok(cardResponse);
    }

    [HttpPost]
    public async Task<ActionResult<Badge>> CreateCard(BadgeRequest badge)
    {
      var newBadge = new BadgeExosRequest
      {
        BadgeName = badge.BadgeName,
        MediaDefinitionFk = 1,
        MediaRoleAuthorisation = "All",
        ApplicationDefinitions = new List<ApplicationDefinition>
        {
          new ApplicationDefinition
          {
            BadgeNumber = new Random().Next(2147483647).ToString(),
            ApplicationDefinitionFk = 1
          }
        }
      };
      await _client.PostAsync($"{_url}/api/v1.0/badges/create", ByteMaker(newBadge));

      return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCard(string badgeName)
    {
      var deleteRequest = new BadgeDeleteRequest
      {
        BadgeName = badgeName
      };
      await _client.PostAsync($"{_url}/api/v1.0/badges/delete", ByteMaker(deleteRequest));
      return NoContent();
    }
  }
}
