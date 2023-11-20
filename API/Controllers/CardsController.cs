using System.Text;
using Microsoft.Identity.Client;

namespace SLAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CardsController : ControllerBase
{
  private static PersonsController _personClient = default!;
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _badgeUrlStart;
  private readonly string? _badgeUrlEnd;
  private readonly SourceRepository _exosService;
  private readonly AccessContext _context;

  public CardsController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _badgeUrlStart = config.GetValue<string>("Url:GetBadgeStart");
    _badgeUrlEnd = config.GetValue<string>("Url:GetBadgeEnd");
    _exosService = new SourceRepository(_client, context);
    _context = context;
  }

  [HttpGet("{badgeName?}")]
  public async Task<ActionResult<List<ReceiverBadgeResponse>>> GetCard(string? badgeName)
  {
    try
    {
      var objectResult = await _exosService.GetSource($"{_url}{_badgeUrlStart}{badgeName}{_badgeUrlEnd}");
      var card = JsonConvert.DeserializeObject<List<SourceBadgeResponse>>(objectResult["value"]!.ToString())!.FirstOrDefault();

      var cardResponse = new ReceiverBadgeResponse
      {
        CardNumber = card!.BadgeName,
        PersonPrimaryId = card.Person.PersonBaseData.PersonalNumber,
        IsReleased = card.MediaUsageData.ReleaseState == "Released" ? true : false,
        Origin = "A",
        ValidTo = card.ValidTo,
        ValidFrom = card.ValidFrom,
        LastModified = card.LastChangeDate,
      };

      return Ok(cardResponse);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPost]
  public async Task<ActionResult<string>> CreateCard(ReceiverBadgeRequest badge)
  {
    try
    {
      var newBadge = new SourceBadgeRequest
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
        var personId = await _context.PersonNumberLink.FirstOrDefaultAsync(x => x.EmployeeNumber == badge.PersonalNumber);
        var person = await _personClient.GetPerson(badge.PersonalNumber);
        if (person.Result is NotFoundResult) return NotFound();
        var personResponse = ((OkObjectResult)person.Result!).Value as ReceiverPersonResponse;
        await _client.PostAsync($"{_url}/api/v1.0/persons/{personId!.PersonalId}/assignBadge?ignoreBlacklist=false", ByteMaker(new { BadgeName = badge.CardNumber }));

      }
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteCard(ReceiverBadgeRequest badgeName)
  {
    var deleteRequest = new SourceBadgeDeleteRequest
    {
      BadgeName = badgeName.CardNumber
    };
    await _client.PostAsync($"{_url}/api/v1.0/badges/delete", ByteMaker(deleteRequest));
    return NoContent();
  }
}

