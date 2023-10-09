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
    _exosService = new ExosRepository(_client, context);
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
    return _exosService.ExosDbGetAccessRights();
  }
  [HttpPost]
  public async Task<ActionResult> PostAccessRight(BetsyAccessRightRequest request)
  {
    return Ok(await _exosService.ExosPostAccessRightDb(request));
  }
  [HttpDelete]
  public ActionResult DeleteDbAccessRight(string id)
  {
    _exosService.ExosDeletAccessRight(id);
    return NoContent();
  }

}

