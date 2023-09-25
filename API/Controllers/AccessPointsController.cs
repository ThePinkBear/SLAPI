using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Test.Models;

namespace SLAPI.Controllers
{
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

      var accessPointResponses =
        from accesspoint in devices
        select new BetsyAccessPointResponse
        {
          AccessPointId = accesspoint.Id,
          Address = accesspoint.Address,
          Description = accesspoint.AccessPointId
        };
      
      if (!String.IsNullOrEmpty(accessPointId))
      { 
        var result = accessPointResponses
                        .Where(x => x.AccessPointId == accessPointId)
                        .Select(x => x).FirstOrDefault();
        return result == null 
                          ? NotFound()
                          : Ok(result);                                        
      }
      
      return accessPointResponses == null ? NotFound() : Ok(accessPointResponses);
    }

    [HttpPut("{id} {command}")]
    public async Task<IActionResult> PutAccessPoint(string id, string command)
    {
      if ((await GetAccessPoints(id)).Result is NotFoundResult) return BadRequest("No such access point");
      if (command.ToLower() is "open")
      {
        var response = await _client.PostAsync($"{_url}/sysops/v1.0/{id}/command/ReleaseOnce/", null);
        return response.IsSuccessStatusCode ? NoContent() : BadRequest();
      }
      return BadRequest("No such command");
    }
  }
}
