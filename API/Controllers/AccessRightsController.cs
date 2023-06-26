using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccessRightsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;

    public AccessRightsController(HttpClient client, IConfiguration config)
    {
      _client = client;
      _url = config.GetValue<string>("ExosUrl");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccessRight>> GetAccessRight(string id)
    {
      var accessRights = await _client.GetFromJsonAsync<List<AccessRight>>($"{_url}/v1.0/accessrights");

      var accessRight = accessRights == null ? null : accessRights.FirstOrDefault(x => x.PersonPrimaryId == id);

      return accessRight == null ? NotFound() : accessRight;
    }
    
    [HttpPost]
    public Task<ActionResult<Person>> AssignAccessRight(AccessRightCreateRequest accessRight)
    {
      throw new NotImplementedException();

      // TODO: Assign Accessright here??.
      // var newAccessRight = new AccessRight
      // {
      //   BadgeId = accessRight.BadgeId,
      //   BadgeName = accessRight.BadgeName,
      //   PersonPrimaryId = accessRight.PersonPrimaryId
      // };
    
      // var createdAccessRight = await _client.PostAsJsonAsync($"{_url}/v1.0/accessrights/create", newAccessRight);

      // return CreatedAtAction("GetAccessRight", new { id = accessRight.PersonPrimaryId }, createdAccessRight);
    }
  }
}
