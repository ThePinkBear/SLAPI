using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Test.Models;
using System.Linq;

namespace SLAPI.Controllers
{
  [Route("api/r/[controller]")]
  [ApiController]
  public class PersonController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _personUrl1;
    private readonly string? _personUrl2;

    public PersonController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _personUrl1 = config.GetValue<string>("Url:rPersonStart");
      _personUrl2 = config.GetValue<string>("Url:rPersonEnd");
    }

    [HttpGet]
    public async Task<ActionResult<List<AccessRightResponse>>> GetPerson(string personalNumber)
    {
      var response = await _client.GetAsync($"{_url}{_personUrl1}{personalNumber}{_personUrl2}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var person = new ExosPerson();
      try
      {
        person = JsonConvert.DeserializeObject<ExosPerson>(objectResult["value"]![0]!.ToString());
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound($"No person with this Id: {personalNumber} found");
      }

      var result = new List<AccessRightResponse>();
    
        result.AddRange(from accessRight in person?.PersonAccessControlData.accessRights
                        select new AccessRightResponse()
                        {
                          AccessPointId = accessRight.BadgeId,
                          PersonPrimaryId = accessRight.BadgeName,
                          ScheduleId = accessRight.TimeZoneIdInternal
                        });
      
      return Ok(result); // TODO Check if empty array or 404 as return for no accessrights found
    }


  }
}
