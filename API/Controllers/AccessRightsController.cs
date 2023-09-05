using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    public AccessRightsController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _apiUrl = config.GetValue<string>("ExosUrl");
      _accessRightUrl = config.GetValue<string>("Url:AccessRights");
    }

    [HttpGet]
    public async Task<ActionResult<AccessRightResponse>> GetAccessRight()
    {
      var response = await _client.GetAsync($"{_apiUrl}{_accessRightUrl}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var accessRights = JsonConvert.DeserializeObject<List<AccessRight>>(objectResult["value"]!.ToString());

      var accessRightResponse =
        from ar in accessRights
        select new AccessRightResponse
        {
          AccessPointId = ar.BadgeId,
          ScheduleId = ar.TimeZoneIdInternal,
          PersonPrimaryId = ar.PersonPrimaryId
        };

      return accessRightResponse == null ? NotFound() : Ok(accessRightResponse);
    }
    
    // [HttpPost]
    // public async Task<ActionResult<Person>> AssignAccessRight(AccessRightRequest accessRight)
    // {
    //   var newAccessRight = new AccessRight
    //   {
    //     BadgeId = accessRight.TimeZoneId,
    //     BadgeName = accessRight.BadgeName,
    //     PersonPrimaryId = accessRight.PersonPrimaryId
    //   };
    
    //   var response = await _client.PostAsJsonAsync($"{_apiUrl}{_accessRightUrl}/create", newAccessRight);

    //   return !response.IsSuccessStatusCode ? StatusCode(500) : NoContent();
    // }
  }
}
