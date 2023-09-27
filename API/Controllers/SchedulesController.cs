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
    private readonly ExosRepository _exosService;

    public SchedulesController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _scheduleUrl = config.GetValue<string>("Url:Schedule");
      _exosService = new ExosRepository();
    }

    [HttpGet]
    public async Task<ActionResult<List<BetsyScheduleResponse>>> GetSchedules()
    {
      try
      {
        var response = await _exosService.GetExos<ExosScheduleResponse>(_client, $"{_url}{_scheduleUrl}", "value");

        var schedules = from schedule in response
                        select new BetsyScheduleResponse
                        {
                          ScheduleId = schedule.TimeZoneId,
                          Description = schedule.DisplayName
                        };

        return schedules == null ? NotFound() : Ok(schedules);
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
