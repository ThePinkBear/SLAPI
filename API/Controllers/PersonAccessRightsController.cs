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
    private readonly ExosRepository _exosService;

    public PersonController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _personUrl1 = config.GetValue<string>("Url:rPersonStart");
      _personUrl2 = config.GetValue<string>("Url:rPersonEnd");
      _exosService = new ExosRepository();
    }

    [HttpGet]
    public ActionResult<List<BetsyAccessRightResponse>> GetPerson(string personalNumber)
    {
      var objectResponse = _exosService.GetExos(_client, $"{_url}{_personUrl1}{personalNumber}{_personUrl2}").Result;

      try
      {
        var person = JsonConvert.DeserializeObject<ExosPerson>(objectResponse["value"]![0]!.ToString());
        var result = new List<BetsyAccessRightResponse>();

        result.AddRange(from accessRight in person?.PersonAccessControlData.accessRights
                        select new BetsyAccessRightResponse()
                        {
                          AccessPointId = accessRight.BadgeId,
                          PersonPrimaryId = accessRight.BadgeName,
                          ScheduleId = accessRight.TimeZoneIdInternal
                        });

        return Ok(result);
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound($"No person with this Id: {personalNumber} found");
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
