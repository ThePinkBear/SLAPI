using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Test.Models;

namespace SLAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SchedulesController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _scheduleUrl;

    public SchedulesController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _scheduleUrl = config.GetValue<string>("Url:Schedule");
    }

    [HttpGet]
    public async Task<ActionResult<List<BetsyScheduleResponse>>> GetSchedules()
    {
      var response = await _client.GetAsync($"{_url}{_scheduleUrl}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var schedules = JsonConvert.DeserializeObject<List<BetsyScheduleResponse>>(objectResult["value"]!.ToString());

      return schedules == null ? NotFound() : Ok(schedules);
    }
  }
}
