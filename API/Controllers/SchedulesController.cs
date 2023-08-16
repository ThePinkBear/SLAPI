using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SchedulesController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _scheduleUrl;
    private readonly string _credentials;

    public SchedulesController(IHttpClientFactory client, IConfiguration config)
    {
      var c = new Credentials(config);
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _scheduleUrl = config.GetValue<string>("Url:Schedule");
      _credentials = c.Value;
    }

    [HttpGet]
    public async Task<ActionResult<List<Schedule>>> GetSchedules()
    {
      _client.DefaultRequestHeaders.Authorization = new System.Net.Http
              .Headers.AuthenticationHeaderValue("Basic", _credentials);

      var response = await _client.GetAsync($"{_url}{_scheduleUrl}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var schedules = JsonConvert.DeserializeObject<List<Schedule>>(objectResult["value"]!.ToString());

      return schedules == null ? NotFound() : Ok(schedules);
    }
  }
}
