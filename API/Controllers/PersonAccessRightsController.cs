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
  private readonly SourceRepository _repo;
  private readonly AccessContext _context;

  public PersonController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _personUrl1 = config.GetValue<string>("Url:rPersonStart");
    _personUrl2 = config.GetValue<string>("Url:rPersonEnd");
    _repo = new SourceRepository(_client, context);
    _context = context;
  }

  [HttpGet("{personalNumber}")]
  public async Task<ActionResult<List<ReceiverAccessRightResponse>>> GetPersonAccessRights(string personalNumber)
  {
    var objectResponse = _repo.GetSource($"{_url}{_personUrl1}{personalNumber}{_personUrl2}").Result;

    try
    {
      var person = JsonConvert.DeserializeObject<Value>(objectResponse["value"]![0]!.ToString());

      try
      {
        if (person?.PersonAccessControlData.AccessRights == null) return NotFound($"No person with this Id: {personalNumber} found");
        await _context.Database.ExecuteSqlAsync($"TRUNCATE TABLE [Requests]");
        await _context.SaveChangesAsync();

        _context.Requests.AddRange(from accessRight in person?.PersonAccessControlData.AccessRights select new DbUnassignRequest()
                          {
                            AssignMentId = accessRight.AssignmentId,
                            PersonId = person!.PersonBaseData.PersonId,
                            AccessRightId = accessRight.AccessRightId
                          });
        await _context.SaveChangesAsync();
      }
      catch (DBConcurrencyException ex)
      {
        return StatusCode(500, ex.Message);
      }

      var result = new List<ReceiverAccessRightResponse>();

      result.AddRange(from accessRight in person?.PersonAccessControlData.AccessRights
                      select new ReceiverAccessRightResponse()
                      {
                        rid = _context.Requests.FirstOrDefault(x => x.AssignMentId == accessRight.AssignmentId)!.Id,
                        aid = accessRight.AccessRight.DisplayName,
                        sid = "Always"
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

