using Microsoft.AspNetCore.Mvc;
using Test.Models;

namespace SLAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccessRightsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _apiUrl;
    private readonly string? _accessRightUrl;

    public AccessRightsController(HttpClient client, IConfiguration config)
    {
      _client = client;
      _apiUrl = config.GetValue<string>("ExosUrl");
      _accessRightUrl = config.GetValue<string>("Url:AccessRights");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccessRight>> GetAccessRight(string id)
    {
      var accessRights = await _client.GetFromJsonAsync<List<AccessRight>>($"{_apiUrl}{_accessRightUrl}");

      var accessRight = accessRights?.FirstOrDefault(x => x.PersonPrimaryId == id);

      return accessRight == null ? NotFound() : accessRight;
    }
    
    [HttpPost]
    public async Task<ActionResult<Person>> AssignAccessRight(AccessRightRequest accessRight)
    {
      var newAccessRight = new AccessRight
      {
        BadgeId = accessRight.TimeZoneId,
        BadgeName = accessRight.BadgeName,
        PersonPrimaryId = accessRight.PersonPrimaryId
      };
    
      var response = await _client.PostAsJsonAsync($"{_apiUrl}{_accessRightUrl}/create", newAccessRight);

      return !response.IsSuccessStatusCode ? StatusCode(500) : NoContent();
    }
  }
}
