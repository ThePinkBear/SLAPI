namespace SLAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
  private readonly SourceRepository _exosService;
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _personUrlStart;
  private readonly string? _personUrlEnd;
  private readonly string? _deleteUrl;
  private readonly string? _createUrl;
  private readonly AccessContext _context;

  public PersonsController(IHttpClientFactory client, IConfiguration config, AccessContext context)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _personUrlStart = config.GetValue<string>("Url:GetOnePersonStart");
    _personUrlEnd = config.GetValue<string>("Url:GetOnePersonEnd");
    _deleteUrl = config.GetValue<string>("Url:DeletePerson");
    _createUrl = config.GetValue<string>("Url:CreatePerson");
    _exosService = new SourceRepository(_client, context);
    _context = context;
  }

  [HttpGet("{personalNumber}")]
  public async Task<ActionResult<ReceiverPersonResponse>> GetPerson(string personalNumber)
  {
    try
    {
      var objectResult = await _exosService.GetSource($"{_url}{_personUrlStart}{personalNumber}{_personUrlEnd}");
      var person = JsonConvert.DeserializeObject<SourcePersonResponse>(objectResult["value"]![0]!.ToString());

      if (_context.PersonNumberLink.FirstOrDefault(x => x.EmployeeNumber == person!.PersonBaseData.PersonalNumber) == null)
      {
        _context.PersonNumberLink.Add(new PersonNumberDB
        {
          EmployeeNumber = person!.PersonBaseData.PersonalNumber,
          PersonalId = person.PersonBaseData.PersonId
        });
        await _context.SaveChangesAsync();
      }

      object? isEnabled = person!.PersonAccessControlData.IsEnabled switch
      {
        "Released" => true,
        "Blocked" => false,
        _ => null
      };
      
      var personResponse = new ReceiverPersonResponse
      {
        FullName = person!.PersonBaseData.Fullname,
        PrimaryId = person.PersonBaseData.PersonalNumber,
        FirstName = person.PersonBaseData.FirstName,
        LastName = person.PersonBaseData.LastName,
        PersonNumber = person.PersonTenantFreeFields.PersonNumber,
        Phone = person.PersonBaseData.PhoneNumber,
        PersonType = person.PersonTenantFreeFields.PersonType,
        Company = person.PersonTenantFreeFields.other,
        Department = person.PersonBaseData.Department,
        PinCode = null,
        IsEnabled = isEnabled!,
        Origin = "A",
        LastModified = person.PersonBaseData.LastModified
      };


      return personResponse == null ? NotFound() : Ok(personResponse);
    }
    catch (ArgumentOutOfRangeException)
    {
      return NotFound();
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }


  [HttpPut]
  public async Task<IActionResult> PutPerson(string personalNumber, ReceiverPersonPutRequest personRequest)
  {
    if (String.IsNullOrEmpty(personalNumber) || personRequest == null) return BadRequest();

    var UpdatedPerson = new SourcePersonRequest
    {
      PersonBaseData = new PersonBaseData
      { // If not necessary, use betsy object directly.
        PersonalNumber = personRequest.PersonalNumber,
        FirstName = personRequest.FirstName,
        LastName = personRequest.LastName,
        PhoneNumber = personRequest!.PhoneNumber,
        Department = personRequest.Department
      }
    };

    try
    {
      var personToEdit = await _context.PersonNumberLink.FirstOrDefaultAsync(x => x.EmployeeNumber == personalNumber);
      var response = await _client.PostAsync($"{_url}/api/v1.0/persons/{personToEdit!.PersonalId}/update?ignoreBlacklist=false", ByteMaker(UpdatedPerson));

      if (!String.IsNullOrEmpty(personRequest.PinCode))
      {
        await _client.PostAsync($"{_url}/api/v1.0/persons/{personToEdit!.PersonalId}/setPin", ByteMaker(personRequest.PinCode));
      }
      return Ok(personRequest.PersonalNumber);
    }
    catch (Exception ex)
    {
      return StatusCode(409, ex.Message);
    }
  }

  [HttpPost]
  public async Task<ActionResult> PostPerson(ReceiverPersonCreateRequest person)
  {
    var createdPerson = new PersonBaseData
    {
      PersonalNumber = person.PersonalNumber,
      FirstName = person.FirstName ?? "",
      LastName = person.LastName ?? "",
      PhoneNumber = person.PhoneNumber ?? "",
      Department = person.Department ?? ""
    };
    var exosPerson = new SourcePersonRequest
    {
      PersonBaseData = createdPerson
    };
    try
    {
      var posted = await _client.PostAsync($"{_url}{_createUrl}", ByteMaker(exosPerson));

      if (posted.IsSuccessStatusCode && !String.IsNullOrEmpty(person.PinCode))
      {
        var response = await GetPerson(person.PersonalNumber!);
        var personalNumber = await _context.PersonNumberLink.FirstOrDefaultAsync(x => x.EmployeeNumber == person.PersonalNumber);
        await _client.PostAsync($"{_url}/api/v1.0/persons/{personalNumber!.PersonalId}/setPin", ByteMaker(person.PinCode));
      }
      return Ok(person.PersonalNumber);
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }

  [HttpDelete("{personalNumber}")]
  public async Task<IActionResult> DeletePerson(string personalNumber)
  {
    var personalNumberId = await _context.PersonNumberLink.FirstOrDefaultAsync(x => x.EmployeeNumber == personalNumber);
    if (String.IsNullOrEmpty(personalNumberId!.PersonalId)) return NotFound();

    var response = await _client.PostAsync($"{_url}{_deleteUrl}{personalNumberId.PersonalId}/delete?checkOnly=false", null);

    return !response.IsSuccessStatusCode ? StatusCode(500) : NoContent();
  }
}
