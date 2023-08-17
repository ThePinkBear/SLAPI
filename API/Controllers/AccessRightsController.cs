using Microsoft.AspNetCore.Mvc;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccessRightsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _apiUrl;
    private readonly string? _accessRightUrl;
    private readonly string _credentials;

    public AccessRightsController(HttpClient client, IConfiguration config)
    {
      var c = new Credentials(config);
      _client = client;
      _apiUrl = config.GetValue<string>("ExosUrl");
      _accessRightUrl = config.GetValue<string>("Url:AccessRights");
      _credentials = c.Value;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccessRight>> GetAccessRight(string id)
    {
      _client.DefaultRequestHeaders.Authorization = 
        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _credentials);

      var accessRights = 
        await _client.GetFromJsonAsync<List<AccessRight>>($"{_apiUrl}{_accessRightUrl}");

      var accessRight = accessRights?.FirstOrDefault(x => x.PersonPrimaryId == id);

      return accessRight == null ? NotFound() : accessRight;
    }
    
    [HttpPost]
    public async Task<ActionResult<Person>> AssignAccessRight(AccessRightCreateRequest accessRight)
    {
      // TODO: Assign Accessright here??.
      var newAccessRight = new AccessRight
      {
        BadgeId = accessRight.BadgeId,
        BadgeName = accessRight.BadgeName,
        PersonPrimaryId = accessRight.PersonPrimaryId
      };
    
      var response = await _client.PostAsJsonAsync($"{_apiUrl}{_accessRightUrl}/create", newAccessRight);

      return !response.IsSuccessStatusCode ? StatusCode(500) : NoContent();
      // return CreatedAtAction("GetAccessRight", new { id = accessRight.PersonPrimaryId }, createdAccessRight);
    }
  }
}
