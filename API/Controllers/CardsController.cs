namespace SLAPI.Controllers;

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
    try
    {
      var cards = await _exosService.GetExos<ExosBadgeResponse>(_client, $"{_url}{_cardUrl1}{badgeName}{_cardUrl2}", "value");

      var cardResponse = from c in cards
                         select new BetsyBadgeResponse
                         {
                           CardNumber = c!.BadgeName,
                          //  PersonPrimaryId = c.Person.PersonBaseData.PersonalNumber
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
        // Person = new ExosPerson
        // {
        //   PersonBaseData = new Person
        //   {
        //     PersonalNumber = ""
        //   }
        // }
      };
      await _client.PostAsync($"{_url}/api/v1.0/badges/create", ByteMaker(newBadge));
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

