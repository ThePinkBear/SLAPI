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

    public AccessRightsController(HttpClient client)
    {
      _client = client;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccessRight>> GetAccessRight(string id)
    {
      var accessRights = await _client.GetFromJsonAsync<List<AccessRight>>($"v1.0/accessrights");

      var accessRight = accessRights == null ? null : accessRights.FirstOrDefault(x => x.PersonPrimaryId == id);

      return accessRight == null ? NotFound() : accessRight;
    }
    
    [HttpPost]
    public async Task<ActionResult<Person>> CreateCard(AccessRightCreateRequest accessRight)
    {
      var newAccessRight = new AccessRight
      {
        BadgeId = accessRight.BadgeId,
        BadgeName = accessRight.BadgeName,
        PersonPrimaryId = accessRight.PersonPrimaryId
      };
    
      var createdAccessRight = await _client.PostAsJsonAsync("v1.0/accessrights/create", newAccessRight);

      return CreatedAtAction("GetAccessRight", new { id = accessRight.PersonPrimaryId }, createdAccessRight);
    }
  }
}
