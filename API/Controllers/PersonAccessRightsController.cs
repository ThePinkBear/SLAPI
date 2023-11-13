using System.Data;
using System.IO.Compression;

namespace SLAPI.Controllers;

[Route("api/r/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _personUrl1;
  private readonly string? _personUrl2;
  private readonly ExosRepository _exosService;
  private readonly AccessContext _context;

  public PersonController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _personUrl1 = config.GetValue<string>("Url:rPersonStart");
    _personUrl2 = config.GetValue<string>("Url:rPersonEnd");
    _exosService = new ExosRepository(_client, context);
    _context = context;
  }

  [HttpGet("{personalNumber}")]
  public async Task<ActionResult<List<BetsyAccessRightResponse>>> GetPersonAccessRights(string personalNumber)
  {
    var objectResponse = _exosService.GetExos($"{_url}{_personUrl1}{personalNumber}{_personUrl2}").Result;

    try
    {
      var person = JsonConvert.DeserializeObject<ExosPersonResponse>(objectResponse["value"]![0]!.ToString());

      try
      {
        _context.Requests.RemoveRange(_context.Requests.Where(r => r.PersonId == person!.PersonBaseData.PersonId).Select(r => r).ToList());
        await _context.SaveChangesAsync();
        _context.Requests.AddRange(from accessRight in person?.PersonAccessControlData.accessRights select new ExosUnassignRequest()
                          {
                            AssignMentId = accessRight.AssignmentId,
                            PersonId = person!.PersonBaseData.PersonId
                          });
        await _context.SaveChangesAsync();
      }
      catch (DBConcurrencyException ex)
      {
        return StatusCode(500, ex.Message);
      }

      var result = new List<BetsyAccessRightResponse>();

      result.AddRange(from accessRight in person?.PersonAccessControlData.accessRights
                      select new BetsyAccessRightResponse()
                      {
                        AccessRightId = accessRight.AssignmentId,
                        AccessPointId = accessRight.AccessRightId,
                        PersonPrimaryId = person!.PersonBaseData.PersonalNumber,
                        ScheduleId = accessRight.TimeZoneId
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

