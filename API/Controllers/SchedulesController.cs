using Microsoft.AspNetCore.Mvc;
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
    private readonly string? _credentials;

    public SchedulesController(HttpClient client, IConfiguration config)
    {
      var c = new Credentials(config);
      _client = client;
      _url = config.GetValue<string>("ExosUrl");
      _scheduleUrl = config.GetValue<string>("Url:Schedule");
      _credentials = c.Value;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
    {
      _client.DefaultRequestHeaders.Authorization = new System.Net.Http
             .Headers.AuthenticationHeaderValue("Basic", _credentials);
             
      var schedules = await _client.GetFromJsonAsync<List<Schedule>>($"{_url}{_scheduleUrl}");

      return schedules == null ? NotFound() : schedules;
    }
  }
}
