using Microsoft.AspNetCore.Mvc;
using Test.Models;
using System.Net.Http;
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
    private string _credentials;
    public string username = "MyApiKey";
    public string password = "1efa7cd3-cf3a-469c-c088-45fc71eacec8";

    public AccessPointsController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _credentials = Convert.ToBase64String
      (
        Encoding.ASCII
        //.GetBytes($"{config.GetValue<string>("ExosUsername")}:{config.GetValue<string>("ExosPassword")}")
        .GetBytes($"{username}:{password}")
      );
    }

    [HttpGet]
    public async Task<IActionResult> GetAccessPoints()
    {
      _client.DefaultRequestHeaders.Authorization = new System.Net.Http
              .Headers.AuthenticationHeaderValue("Basic", _credentials);
      var response = await _client.GetAsync($"{_url}/v1.0/accessRights");
      var responseContent = await response.Content.ReadAsStringAsync();

      var accessPoints = JsonConvert.DeserializeObject<List<AccessPoint>>(responseContent);

      return accessPoints == null ? NotFound() : Ok(accessPoints);
    }
  }
}
