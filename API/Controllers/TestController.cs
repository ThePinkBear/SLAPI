namespace SLAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _scheduleUrl;
  private readonly ExosRepository _exosService;
  private readonly AccessContext _context;

  public TestController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _scheduleUrl = config.GetValue<string>("Url:Schedule");
    _exosService = new ExosRepository();
    _context = context;
  }

  [HttpGet]
  public string GetTest()
  {
    return "Hello World";
  }
  [HttpGet("AccessRights")]
  public List<BetsyAccessRightResponse> GetDBAccessRights()
  {
    var accessRights = _context.AccessRights;

    var result = from accessRight in accessRights select new BetsyAccessRightResponse {
      PersonPrimaryId = accessRight.PersonPrimaryId,
      AccessPointId = accessRight.AccessPointId,
      ScheduleId = accessRight.ScheduleId,
      AccessRightId = accessRight.UniqueId
     };

    return result.ToList();
  }
  [HttpPost]
  public async Task<ActionResult> PostAccessRight(BetsyAccessRightRequest request)
  {
    var newAr = new AccessRightDbObject
    {
      UniqueId = Guid.NewGuid().ToString(),
      PersonPrimaryId = request.PersonPrimaryId,
      ScheduleId = request.TimeZoneId,
      AccessPointId = request.AccessPointId
    };

    _context.AccessRights.Add(newAr);

    await _context.SaveChangesAsync();
    return Ok(newAr.UniqueId);
  }
  [HttpDelete]
  public async Task<ActionResult> DeleteDbAccessRight(string id)
  {
    var accessRight = _context.AccessRights.Where(a => a.UniqueId == id).First();
    _context.AccessRights.Remove(accessRight);
    await _context.SaveChangesAsync();
    return NoContent();
  }

}

