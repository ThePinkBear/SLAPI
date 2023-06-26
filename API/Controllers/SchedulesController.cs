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

        public SchedulesController(HttpClient client)
        {
          _client = client;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
        {
          var schedules = await _client.GetFromJsonAsync<List<Schedule>>($"v1.0/schedules");

          return schedules == null ? NotFound() : schedules;
        }
    }
}
