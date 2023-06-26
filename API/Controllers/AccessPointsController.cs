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
        private readonly string? _url;

      public AccessPointsController(HttpClient client, IConfiguration config)
      {
        _client = client;
        _url = config.GetValue<string>("ExosUrl");
      }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessPoint>>> GetAccessPoints()
        {
          var accessPoints = await _client.GetFromJsonAsync<List<AccessPoint>>($"{_url}/v1.0/accesspoints");

          return accessPoints == null ? NotFound() : accessPoints;
        }
    }
}
