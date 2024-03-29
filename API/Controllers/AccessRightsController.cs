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

  [HttpPost]
  public async Task<ActionResult> AssignAccessRight(ReceiverAccessRightRequest accessRight)
  {
    try
    {
      var personObject = await _repo.GetSource($"{_url}{_personUrl1}{accessRight.PersonPrimaryId}{_personUrl2}");
      var person = JsonConvert.DeserializeObject<Root>(personObject.ToString())!.Value.FirstOrDefault();
      var personId = person!.PersonBaseData.PersonId;
      var assignment = new SourceAssignmentRequest();

      if (_context.AccessRightMatcher.Where(a => a.aid == accessRight.AccessPointId).Count() == 0)
      {
        var accessRights = await GetAccessRights(_repo, _url!, _accessRightUrl!);
        assignment.AccessRightId = accessRights.Where(a => a.aid == accessRight.AccessPointId)
                            .Single()
                            .rid;
        assignment.TimeZoneId = accessRights.Where(a => a.aid == accessRight.AccessPointId)
                        .Single()
                        .sid;
      }
      else
      {
        assignment.AccessRightId = _context.AccessRightMatcher.Where(a => a.aid == accessRight.AccessPointId)
                            .Single()
                            .rid;
        assignment.TimeZoneId = _context.AccessRightMatcher.Where(a => a.aid == accessRight.AccessPointId)
                        .Single()
                        .sid;
      }

      await _client.PostAsync($"{_url}{_accessRightUrl1}{personId}{_accessRightUrl2}", ByteMaker(assignment));


      return Created("Api/r/person", accessRight);

    }
    catch (ArgumentOutOfRangeException)
    {
      return NotFound();
    }
    catch (NullReferenceException)
    {
      return NotFound();
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }
 
  [HttpPut("{assignmentId}")]
  public ActionResult UpdateAccessRight(int assignmentId, [FromBody] object obj)
  {
    System.IO.File.WriteAllText("C:\\Incoming\\PUTaccessRight.json", $"{assignmentId}{obj}");
    return NoContent();
  }


  [HttpDelete]
  public async Task<ActionResult> DeleteAccessRight(int assignmentId)
  {
    var personId = _context.Requests.Where(r => r.Id == assignmentId).Single();

    var path = $"{_url}/api/v1.1/persons/{personId.PersonId}/unassignAccessRight/{personId.AssignMentId}";

    await _client.PostAsync(path, ByteMaker(new SourceUnassignmentRequest
    {
      AssignMentId = personId.AssignMentId,
      PersonId = personId.PersonId
    }));
    _context.Requests.Remove(personId);
    await _context.SaveChangesAsync();
    return Ok();
  }
}

