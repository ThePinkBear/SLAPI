using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccessPointsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _accessPointUrl;
    private readonly string _credentials;

    public AccessPointsController(IHttpClientFactory client, IConfiguration config)
    {
      var c = new Credentials(config);
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _accessPointUrl = config.GetValue<string>("Url:AccessPoint");
      _credentials = c.Value;
    }

    [HttpGet]
    public async Task<ActionResult<List<AccessPointResponse>>> GetAccessPoints(string? accessPointId)
    {
      _client.DefaultRequestHeaders.Authorization = new System.Net.Http
              .Headers.AuthenticationHeaderValue("Basic", _credentials);

      var response = await _client.GetAsync($"{_url}{_accessPointUrl}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var devices = JsonConvert.DeserializeObject<List<AccessPoint>>(objectResult["Value"]["Devices"].ToString());
      
      var accessPoints = 
        from accesspoint in devices
        select new AccessPointResponse
        {
            AccessPointId = accesspoint.Id,
            Address = accesspoint.Address,
            Description = accesspoint.AccessPointId
        };
      if (accessPointId != null) return accessPoints
                                            .Where(x => x.AccessPointId == accessPointId)
                                            .Select(x => x).FirstOrDefault() == null
                                                ? NotFound()
                                                : Ok(accessPoints
                                                      .Where(x => x.AccessPointId == accessPointId)
                                                      .Select(x => x).FirstOrDefault());

      return accessPoints == null ? NotFound() : Ok(accessPoints);
    }

    [HttpPut("{id} {command}")]
    public async Task<IActionResult> PutAccessPoint(string id, string command)
    {
     _client.DefaultRequestHeaders.Authorization = new System.Net.Http
             .Headers.AuthenticationHeaderValue("Basic", _credentials);
      
     if ((await GetAccessPoints(id)).Result is NotFoundResult) return BadRequest("No such access point");
     
     var response = await _client.PostAsync($"{_url}/sysops/v1.0/{id}/command/{command}/", null);

     return response.IsSuccessStatusCode ? NoContent() : StatusCode(500);
    }
  }
}
