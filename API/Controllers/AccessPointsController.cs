
namespace SLAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccessPointsController : ControllerBase
{
  private readonly SourceRepository _repo;
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _accessPointUrl;

  public AccessPointsController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _accessPointUrl = config.GetValue<string>("Url:AccessPoint");
    _repo = new SourceRepository(_client, context);
  }

  [HttpGet]
  public async Task<ActionResult<List<ReceiverAccessPointResponse>>> GetAccessPoints([FromQuery] string? accessPointId = null)
  {
    var devices = await _repo.GetExos<SourceAccessPointResponse>($"{_url}{_accessPointUrl}", "Value", "Devices");

    var accessPoints =
      from device in devices
      select new ReceiverAccessPointResponse
      {
        AccessPointId = device.AccessPointId,
        Address = device.Address,
        Description = device.AccessPointId
      };

    if (!String.IsNullOrEmpty(accessPointId))
    {
      var accessPoint = accessPoints
                      .Where(x => x.AccessPointId.ToLower() == accessPointId.ToLower())
                      .Select(x => x).FirstOrDefault();
      return accessPoint == null
                        ? NotFound()
                        : Ok(accessPoint);
    }

    return accessPoints == null ? NotFound() : Ok(accessPoints);
  }

  [HttpPut("{id}/open")]
  public async Task<IActionResult> PutAccessPoint(string id)
  {
    if ((await GetAccessPoints(id)).Result is NotFoundResult) return BadRequest("No such accesspoint");

    var response = await _client.PostAsync($"{_url}/sysops/v1.0/{id}/command/ReleaseOnce/", null);
    return response.IsSuccessStatusCode ? NoContent() : BadRequest();
  }
}

