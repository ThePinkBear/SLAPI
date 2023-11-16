namespace SLAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _scheduleUrl;
  private readonly ExosRepository _exosService;

  public SchedulesController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _scheduleUrl = config.GetValue<string>("Url:Schedule");
    _exosService = new ExosRepository(_client, context);
  }

  [HttpGet]
  public ActionResult<List<BetsyScheduleResponse>> GetSchedules()
  {
    // try
    // {
    //   var response = await _exosService.GetExos<ExosScheduleResponse>($"{_url}{_scheduleUrl}", "value");

    //   var schedules = from schedule in response
    //                   select new BetsyScheduleResponse
    //                   {
    //                     ScheduleId = "Always"/*schedule.TimeZoneId*/,
    //                     Description = "Timezone for KABA"/*schedule.DisplayName*/
    //                   };

    //   return schedules == null ? NotFound() : Ok(schedules);
    // }
    // catch (Exception ex)
    // {
    //   return BadRequest(ex.Message);
    // }
    return Ok( new List<BetsyScheduleResponse>(){ new BetsyScheduleResponse { 
      ScheduleId = "Always", 
      Description = "Timezone for KABA" 
      }});
  }
}

