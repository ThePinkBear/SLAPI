namespace SLAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccessRightsController : ControllerBase
{
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _accessRightUrl;
  private readonly string? _accessRightUrl1;
  private readonly string? _accessRightUrl2;
  private readonly string? _personUrl1;
  private readonly string? _personUrl2;
  private readonly AccessContext _context;
  private readonly SourceRepository _repo;

  public AccessRightsController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _context = context;
    _url = config.GetValue<string>("ExosUrl");
    _accessRightUrl = config.GetValue<string>("Url:AccessRight");
    _accessRightUrl1 = config.GetValue<string>("Url:AssignAccessRightStart");
    _accessRightUrl2 = config.GetValue<string>("Url:AssignAccessRightEnd");
    _personUrl1 = config.GetValue<string>("Url:rPersonStart");
    _personUrl2 = config.GetValue<string>("Url:rPersonEnd");
    _repo = new SourceRepository(_client, _context);
  }

  [HttpGet]
  public async Task<ActionResult<List<ReceiverAccessRightResponse>>> HourlyGetAccessRights()
  {
    var accessRightIds = await _repo.GetExos<SourceAccessRightResponse>($"{_url}{_accessRightUrl}", "value");

    Dictionary<string, string> ArIdTzId = new();

    foreach (var ar in accessRightIds)
    {
      var ar2 = await _repo.GetExos<SourceScheduleResponse>($"{_url}/api/v1.0/timeZones?accessRightId={ar.AccessRightId}&%24count=true&%24top=4", "value");

      ArIdTzId.Add(ar.DisplayName, ar2[0].TimeZoneId);
    }

    List<ReceiverAccessRightResponse> accessRights = new();

    foreach (var ar in accessRightIds)
    {
      accessRights.Add(new ReceiverAccessRightResponse
      {
        rid = ar.AccessRightId,
        aid = ar.DisplayName,
        sid = ArIdTzId[ar.DisplayName]
      });
    }
    return Ok(accessRights);
  }

  // [HttpPost]
  // public void PostAccessRight([FromBody]object obj)
  // {
  //   System.IO.File.WriteAllText("C:\\Incoming\\POSTaccessRight.json", $"{obj}");
  // }
  [HttpPost]
  public async Task<ActionResult> AssignAccessRight(ReceiverAccessRightRequest accessRight)
  {
    try
    {
      var objectResult = await _repo.GetSource($"{_url}{_personUrl1}{accessRight.PersonPrimaryId}{_personUrl2}");
      var personId = JsonConvert.DeserializeObject<Value>(objectResult["value"]![0]!.ToString())!.PersonBaseData.PersonId;
      var assignment = new SourceAssignmentRequest
      {
        AccessRightId = accessRight.AccessPointId,
        TimeZoneId = accessRight.TimeZoneId
      };
      
      await _client.PostAsync($"{_url}{_accessRightUrl1}{personId}{_accessRightUrl2}", ByteMaker(assignment));
      return Ok(personId);
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
  [HttpPut("{assignmentId}")]
  public ActionResult UpdateAccessRight(string assignmentId, [FromBody]object obj)
  {
    System.IO.File.WriteAllText("C:\\Incoming\\PUTaccessRight.json", $"{obj}");
    return NoContent();
  }
  [HttpDelete]
  public async Task<ActionResult> DeleteAccessRight(string assignmentId)
  {
    string? personId = _context.Requests.Where(r => r.AssignMentId == assignmentId).Select(p => p.PersonId).Single();


    var path = $"{_url}/api/v1.1/persons/{personId}/unassignAccessRight/{assignmentId}";

    await _client.PostAsync(path, ByteMaker(new SourceUnassignmentRequest{
      AssignMentId = assignmentId,
      PersonId = personId
     }));
    return Ok();
  }

}

