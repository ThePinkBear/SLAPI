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

    public SchedulesController(HttpClient client, IConfiguration config)
    {
      _client = client;
      _url = config.GetValue<string>("ExosUrl");
      _scheduleUrl = config.GetValue<string>("Url:Schedule");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
    {
      var schedules = await _client.GetFromJsonAsync<List<Schedule>>($"{_url}{_scheduleUrl}");

      return schedules == null ? NotFound() : schedules;
    }
  }
}
