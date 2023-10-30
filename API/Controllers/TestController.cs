namespace SLAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
  private readonly HttpClient _client;

  private readonly ExosRepository _exosService;
  private readonly ILogger<AccessPointsController> _logger;
  private readonly AccessContext _context;

  public TestController(IHttpClientFactory client, AccessContext context, ILogger<AccessPointsController> logger)
  {
    _client = client.CreateClient("ExosClientDev");
    _exosService = new ExosRepository(_client, context);
    _logger = logger;
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
  [HttpGet("requestMigrations")]
  public ActionResult Migrations()
  {
    using (var db = _context)
    {
      db.Database.Migrate();
    }
    return Ok("Migration Complete");
  }
  [HttpPost]
  public async Task<ActionResult> PostAccessRight(BetsyAccessRightRequest request)
  {
    return Ok(await _exosService.ExosPostAccessRightDb(request));
  }
  [HttpDelete]
  public ActionResult DeleteDbAccessRight(string id)
  {
    _exosService.ExosDeleteAccessRight(id);
    return NoContent();
  }
}

