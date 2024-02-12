namespace SLAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SchedulesController : ControllerBase
{
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _scheduleUrl;
  private readonly SourceRepository _exosService;

  public SchedulesController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _scheduleUrl = config.GetValue<string>("Url:Schedule");
    _exosService = new SourceRepository(_client, context);
  }

  /// <summary>
  /// Implementation is currently hard-coded but a possible implementation is commented out below.
  /// </summary>
  [HttpGet]
  public ActionResult<List<ReceiverScheduleResponse>> GetSchedules()
  {
     return Ok(
      new List<ReceiverScheduleResponse>(){ 
        new ReceiverScheduleResponse 
        { 
          ScheduleId = "Always", 
          Description = "Timezone for KABA" 
        }
      }
    );
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
   
  }
}

