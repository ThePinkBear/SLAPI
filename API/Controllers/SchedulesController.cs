using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly string? _url;

        public SchedulesController(HttpClient client, IConfiguration config)
        {
          _client = client;
          _url = config.GetValue<string>("ExosUrl");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
        {
          var schedules = await _client.GetFromJsonAsync<List<Schedule>>($"{_url}/v1.0/schedules");

          return schedules == null ? NotFound() : schedules;
        }
    }
}
