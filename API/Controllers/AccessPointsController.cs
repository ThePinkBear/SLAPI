using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class AccessPointsController : ControllerBase
    {
        private readonly HttpClient _client;

      public AccessPointsController(HttpClient client)
      {
        _client = client;
      }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessPoint>>> GetAccessPoints()
        {
          var accessPoints = await _client.GetFromJsonAsync<List<AccessPoint>>($"v1.0/accesspoints");

          return accessPoints == null ? NotFound() : accessPoints;
        }
    }
}
