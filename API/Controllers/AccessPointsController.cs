using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Test.Models;
using System.Text.Json;

namespace SLAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccessPointsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _accessPointUrl;

    public AccessPointsController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _accessPointUrl = config.GetValue<string>("Url:AccessPoint");
    }

    [HttpGet]
    public async Task<ActionResult<List<AccessPointResponse>>> GetAccessPoints(string? accessPointId)
    {
      var response = await _client.GetAsync($"{_url}{_accessPointUrl}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var devices = JsonConvert.DeserializeObject<List<AccessPoint>>(objectResult["Value"]!["Devices"]!.ToString());

      var accessPointResponses =
        from accesspoint in devices
        select new AccessPointResponse
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
      if (command.ToLower() is ("open"))
      {
        var response = await _client.PostAsync($"{_url}/sysops/v1.0/{id}/command/ReleaseOnce/", null);
        return response.IsSuccessStatusCode ? NoContent() : BadRequest();
      }
      return BadRequest("No such command");
    }
  }
}
