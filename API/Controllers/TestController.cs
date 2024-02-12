namespace SLAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MaintenanceController : ControllerBase
{
  private readonly HttpClient _client;

  private readonly SourceRepository _exosService;
  private readonly ILogger<AccessPointsController> _logger;
  private readonly AccessContext _context;

  public MaintenanceController(IHttpClientFactory client, AccessContext context, ILogger<AccessPointsController> logger)
  {
    _client = client.CreateClient("ExosClientDev");
    _exosService = new SourceRepository(_client, context);
    _logger = logger;
    _context = context;
  }

  [HttpGet]
  public string GetTest()
  {
    return "Hello World";
  }
  [HttpGet("requestMigrations")]
  public ActionResult Migrations()
  {
    using (var db = _context)
    {
      db.Database.Migrate();
    }
    // Will only return Ok if migration is successful as obove logic throws error if it isn't and a 500 is returned. but always ensure the creation of the db regardless
    return Ok("Migration Complete");
  }


  /// <summary>
  /// Commented out below is the implementation for post and put methods if the incoming object from betsy
  /// needs to be printed out for analysis
  /// </summary>
  /// 
  // [HttpPost]
  // public void Migrations([FromBody]object obj)
  // {
  //   System.IO.File.WriteAllText("C:\\Incoming\\something.json", $"{obj}");
  // }
}

