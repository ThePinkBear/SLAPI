using System.Text;
using Microsoft.Identity.Client;

namespace SLAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CardsController : ControllerBase
{
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _badgeUrlStart;
  private readonly string? _badgeUrlEnd;
  private readonly SourceRepository _repo;
  private readonly AccessContext _context;
  private readonly IConfiguration _config;
  private readonly string? _personUrl1;
  private readonly string? _personUrl2;

  public CardsController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _badgeUrlStart = config.GetValue<string>("Url:GetBadgeStart");
    _badgeUrlEnd = config.GetValue<string>("Url:GetBadgeEnd");
    _personUrl1 = config.GetValue<string>("Url:rPersonStart");
    _personUrl2 = config.GetValue<string>("Url:rPersonEnd");
    _repo = new SourceRepository(_client, context);
    _context = context;
    _config = config;
  }

  [HttpGet("{badgeName?}")]
  public async Task<ActionResult<List<ReceiverBadgeResponse>>> GetCard(string? badgeName)
  {
    try
    {
      var objectResult = await _repo.GetSource($"{_url}{_badgeUrlStart}{badgeName}{_badgeUrlEnd}");
      var card = JsonConvert.DeserializeObject<List<SourceBadgeResponse>>(objectResult["value"]!.ToString())!.FirstOrDefault();

      var cardResponse = new ReceiverBadgeResponse
      {
        CardNumber = card!.BadgeName,
        PersonPrimaryId = card.Person.PersonBaseData.PersonalNumber,
        IsEnabled = card.MediaUsageData.ReleaseState == "Released" ? true : false,
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

  // [HttpPost]
  // public void PostPerson([FromBody]object obj)
  // {
  //   System.IO.File.WriteAllText("C:\\Incoming\\POSTcard.json", $"{obj}");
  // }

  [HttpPost]
  public async Task<ActionResult<string>> CreateCard(ReceiverBadgeRequest badge)
  {
    try
    {
      var newBadge = new SourceBadgeRequest
      {
        BadgeName = badge.CardNumber,
        MediaDefinitionFk = 2,
        MediaRoleAuthorisation = "All",
        ApplicationDefinitions = new List<ApplicationDefinition>
        {
          new ApplicationDefinition
          {
            BadgeNumber = badge.CardNumber,
            ApplicationDefinitionFk = 3
          }
        }
      };

      var response = await _client.PostAsync($"{_url}/api/v1.0/badges/create", ByteMaker(newBadge));
      if (response.IsSuccessStatusCode)
      {
        var personObject = await _repo.GetSource($"{_url}{_personUrl1}{badge.PersonPrimaryId}{_personUrl2}");
        var person = JsonConvert.DeserializeObject<Root>(personObject.ToString())!.Value.FirstOrDefault();
        await _client.PostAsync($"{_url}/api/v1.0/persons/{person!.PersonBaseData.PersonId}/assignBadge?ignoreBlacklist=false", ByteMaker(new { BadgeName = badge.CardNumber }));

      }
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPut("{badgeName}")]
  public IActionResult UpdateCard(string badgeName, [FromBody] object obj)
  {
    System.IO.File.WriteAllText("C:\\Incoming\\PUTcard.json", $"{obj}");
    return NoContent();
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

