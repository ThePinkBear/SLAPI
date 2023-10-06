using Microsoft.Identity.Client;

namespace SLAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CardsController : ControllerBase
{
  private static PersonsController _personClient = default!;
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
    _personClient = new PersonsController(client, config);
  }

  [HttpGet("{badgeName?}")]
  public async Task<ActionResult<List<BetsyBadgeResponse>>> GetCard(string? badgeName)
  {
    try
    {
      // var cards = await _exosService.GetExos<ExosBadgeResponse>(_client, $"{_url}{_cardUrl1}{badgeName}{_cardUrl2}", "value");

      var objectResult = await _exosService.GetExos(_client, $"{_url}/api/v1.0/badges?badgeName={badgeName}");
      var card = JsonConvert.DeserializeObject<ExosBadgeResponse>(objectResult["value"]![0]!.ToString());

      var cardResponse =  new BetsyBadgeResponse
                         {
                           CardNumber = card!.BadgeName,
                           //PersonPrimaryId = card.Person.PersonBaseData.PersonalNumber
                         };

      return Ok(cardResponse);
    }
    catch (Exception ex)
    {
      return Ok(ex.Message);
    }
  }

  [HttpPost]
  public async Task<ActionResult<ExosBadgeResponse>> CreateCard(BetsyBadgeRequest badge)
  {
    try
    {
      var newBadge = new ExosBadgeRequest
      {
        BadgeName = badge.CardNumber,
        MediaDefinitionFk = 1,
        MediaRoleAuthorisation = "All",
        ApplicationDefinitions = new List<ApplicationDefinition>
        {
          new ApplicationDefinition
          {
            BadgeNumber = badge.CardNumber,
            ApplicationDefinitionFk = 1
          }
        }
      };

      var response = await _client.PostAsync($"{_url}/api/v1.0/badges/create", ByteMaker(newBadge));
      if (response.IsSuccessStatusCode)
      {
        var person = await _personClient.GetPerson(badge.PersonalNumber);
        if (person.Result is NotFoundResult) return NotFound();
        var personResponse = ((OkObjectResult)person.Result!).Value as BetsyPersonResponse;
        await _client.PostAsync($"{_url}/api/v1.0/persons/{personResponse!.PersonId}/assignBadge?ignoreBlacklist=false", ByteMaker(new { BadgeName = badge.CardNumber }));

      }
      return NoContent();
    }
    catch (Exception)
    {
      return BadRequest();
    }
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteCard(BetsyBadgeRequest badgeName)
  {
    var deleteRequest = new ExosBadgeDeleteRequest
    {
      BadgeName = badgeName.CardNumber
    };
    await _client.PostAsync($"{_url}/api/v1.0/badges/delete", ByteMaker(deleteRequest));
    return NoContent();
  }
}

