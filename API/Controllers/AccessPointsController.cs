using Microsoft.AspNetCore.Mvc;
using Test.Models;
using System.Text;
using Newtonsoft.Json;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccessPointsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string _credentials;

    public AccessPointsController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _credentials = Convert.ToBase64String
      (
        Encoding.ASCII
        .GetBytes($"{config.GetValue<string>("User:Name")}:{config.GetValue<string>("User:Password")}")
      );
    }

    [HttpGet]
    public async Task<ActionResult<List<AccessPointResponse>>> GetAccessPoints(string? accessPointId)
    {
      _client.DefaultRequestHeaders.Authorization = new System.Net.Http
              .Headers.AuthenticationHeaderValue("Basic", _credentials);
      var response = await _client.GetAsync($"{_url}/sysops/v1.0/peripheryStatusList");
      var responseContent = await response.Content.ReadAsStringAsync();

      var accessPoints = 
        from accesspoint in JsonConvert.DeserializeObject<List<AccessPoint>>(responseContent) select new AccessPointResponse
        {
          AccessPointId = accesspoint.AccessPointId,
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

    [HttpPut("{accessPointId} {command}")]
    public async Task<IActionResult> PutAccessPoint(string accessPointId, string command)
    {
      _client.DefaultRequestHeaders.Authorization = new System.Net.Http
              .Headers.AuthenticationHeaderValue("Basic", _credentials);
      
      if ((await GetAccessPoints(accessPointId)).Result is NotFoundResult) return BadRequest();
      // TODO: Check if a time parameter needs to be added, IE keep open for 3 sek.
      var response = await _client.PostAsync($"{_url}/sysops/v1.0/periphery/{accessPointId}/{command}", null);

      return !response.IsSuccessStatusCode ? StatusCode(500) : NoContent();
    }
  }
}
