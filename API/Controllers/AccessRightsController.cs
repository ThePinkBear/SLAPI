namespace SLAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccessRightsController : ControllerBase
{
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _accessRightUrl1;
  private readonly string? _accessRightUrl2;
  private readonly string? _personUrl1;
  private readonly string? _personUrl2;
  private readonly ExosRepository _exosService;

  public AccessRightsController(IHttpClientFactory client, IConfiguration config)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _accessRightUrl1 = config.GetValue<string>("Url:AssignAccessRightStart");
    _accessRightUrl2 = config.GetValue<string>("Url:AssignAccessRightEnd");
    _exosService = new ExosRepository();
    _personUrl1 = config.GetValue<string>("Url:rPersonStart");
    _personUrl2 = config.GetValue<string>("Url:rPersonEnd");
  }

  [HttpPost("{personalNumber}")]
  public async Task<ActionResult> AssignAccessRight(string personalNumber, BetsyAccessRightRequest accessRight)
  {
    try
    {
      var objectResult = await _exosService.GetExos(_client, $"{_url}{_personUrl1}{personalNumber}{_personUrl2}");
      var personId = JsonConvert.DeserializeObject<ExosPersonResponse>(objectResult["value"]![0]!.ToString())!.PersonBaseData.PersonId;
      var assignment = new ExosAssignmentRequest
      {
        AccessRightId = "",
        TimeZoneId = ""
      };
      // TODO check with Exos what AccessRightID and TimeZoneID is available to the person based on the AdministrationArea they are assigned to and make a response object {AccessRightID, TimeZoneID}
      await _client.PostAsync($"{_url}{_accessRightUrl1}{personId}{_accessRightUrl2}", ByteMaker(assignment));
      return StatusCode(200);
    }
    catch (ArgumentOutOfRangeException)
    {
      return NotFound();
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }
}

