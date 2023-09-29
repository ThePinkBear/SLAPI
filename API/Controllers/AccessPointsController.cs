namespace SLAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccessPointsController : ControllerBase
{
  private readonly ExosRepository _exosService;
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _accessPointUrl;

  public AccessPointsController(IHttpClientFactory client, IConfiguration config)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _accessPointUrl = config.GetValue<string>("Url:AccessPoint");
    _exosService = new ExosRepository();
  }

  [HttpGet]
  public async Task<ActionResult<List<BetsyAccessPointResponse>>> GetAccessPoints(string? accessPointId)
  {
    var devices = await _exosService.GetExos<AccessPoint>(_client, $"{_url}{_accessPointUrl}", "Value", "Devices");

    var accessPoints =
      from device in devices
      select new BetsyAccessPointResponse
      {
        AccessPointId = device.Id,
        Address = device.Address,
        Description = device.AccessPointId
      };

    if (!String.IsNullOrEmpty(accessPointId))
    {
      var accessPoint = accessPoints
                      .Where(x => x.AccessPointId == accessPointId)
                      .Select(x => x).FirstOrDefault();
      return accessPoint == null
                        ? NotFound()
                        : Ok(accessPoint);
    }

    return accessPoints == null ? NotFound() : Ok(accessPoints);
  }

  // TODO THe route in ExosApi "api/AccessPoints/{AccessPointId}/Open/{PrimaryId}" open seems hardcoded, ApId and PId is sent, only bool true or false returned dependant on modelstate.
  [HttpPut("{id}/open")]
  public async Task<IActionResult> PutAccessPoint(string id)
  {
    if ((await GetAccessPoints(id)).Result is NotFoundResult) return BadRequest("No such access point");

    var response = await _client.PostAsync($"{_url}/sysops/v1.0/{id}/command/ReleaseOnce/", null);
    return response.IsSuccessStatusCode ? NoContent() : BadRequest();

  }
}

