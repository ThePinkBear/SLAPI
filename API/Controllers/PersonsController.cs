using System.Text;

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
      var person = JsonConvert.DeserializeObject<Root>(objectResult.ToString())!.Value.FirstOrDefault();

      var DbLink = await _context.PersonNumberLink.FirstOrDefaultAsync(x => x.EmployeeNumber == personalNumber);

      if (DbLink == null)
      {
        // Saves a link between PersonalNumber used by Betsy as UID and the PersonId Exos uses to reduce calls to exos.
        _context.PersonNumberLink.Add(new PersonNumberDB
        {
          EmployeeNumber = person!.PersonBaseData.PersonalNumber,
          PersonalId = person.PersonBaseData.PersonId
        });
        await _context.SaveChangesAsync();
      }

      var isEnabled = person!.PersonAccessControlData.CurrentCardValidity switch
      {
        "Released" => true,
        null => true,
        _ => false
      };

      var personResponse = new ReceiverPersonResponse
      {
        FullName = person!.PersonBaseData.Fullname,
        PrimaryId = person.PersonBaseData.PersonalNumber,
        FirstName = person.PersonBaseData.FirstName,
        LastName = person.PersonBaseData.LastName,
        PersonNumber = person.PersonTenantFreeFields.Text2,
        Phone = person.PersonBaseData.PhoneNumber,
        PersonType = person.PersonTenantFreeFields.Text3,
        Company = person.PersonTenantFreeFields.Text1,
        Department = person.PersonBaseData.Hierarchy,
        PinCode = null,
        IsEnabled = isEnabled,
        Origin = "A",
        LastModified = person.PersonBaseData.LastChangeDate
      };


      return personResponse == null ? NotFound() : Ok(personResponse);
    }
    catch (ArgumentOutOfRangeException)
    {
      return NotFound();
    }
    catch (NullReferenceException)
    {
      return NotFound();
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }
  /// <summary>
  /// Uncomment this endpoint and comment out its implementation to get object structure of incoming object written to file
  /// </summary>
  // [HttpPost]
  // public void PostPerson([FromBody]object obj)
  // {
  //   System.IO.File.WriteAllText($"C:\\Incoming\\{DateTime.Now.ToString("yyyyMMddHHmmss")}POSTperson.json", $"{obj}");
  // }
  
  [HttpPost]
  public async Task<ActionResult> PostPerson(ReceiverPersonCreateRequest person)
  {
    var exosPerson = new SourcePersonRequest
    {
      PersonBaseData = new RequestPersonBaseData
      {
        PersonalNumber = person.PrimaryId,
        FirstName = person.FirstName!,
        LastName = person.LastName!,
        PhoneNumber = person.Phone!
        // Hierarchy = person.Department! Needs to find the FK logic to assign this.
      },
      PersonTenantFreeFields = new PersonTenantFreeFields
      {
        Text1 = person.Company,
        Text2 = person.PersonNumber,
        Text3 = person.PersonType
      }
    };

    var posted = await _client.PostAsync($"{_url}{_createUrl}", ByteMaker(exosPerson));

    /// <summary>
    /// Seting a pincode on a person in Exos is done on a separate endpoint.
    /// This is where the call is made using the PinDecoder class when suitable implementation is discovered.
    /// Implemented version is prepared on line 135, simply remove the empty string "" and uncomment the PinDecoder
    /// method call. 
    /// </summary>
    if (!String.IsNullOrEmpty(person.PinCode) && posted.IsSuccessStatusCode)
    {
      await GetPerson(person.PrimaryId!);
      var personalNumber = await _context.PersonNumberLink.FirstOrDefaultAsync(x => x.EmployeeNumber == person.PrimaryId);
      await _client.PostAsync($"{_url}/api/v1.0/persons/{personalNumber!.PersonalId}/setPin", new StringContent(""/*PinDecoder(person.pinCode) */, Encoding.UTF8, "application/json"));
    }
    if (posted.IsSuccessStatusCode) return Ok(person.PrimaryId);
    return BadRequest();
  }
  
  // [HttpPut("{personalNumber}")]
  // public void PutPerson(string personalNumber, [FromBody]object obj)
  // {
  //   Console.WriteLine(personalNumber);
  //   System.IO.File.WriteAllText("C:\\Incoming\\PUTperson.json", $"{obj}");
  // }
  [HttpPut("{personalNumber}")]
  public async Task<IActionResult> PutPerson(string personalNumber, ReceiverPersonPutRequest personRequest)
  {
    if (String.IsNullOrEmpty(personalNumber) || personRequest == null) return BadRequest();

    var UpdatedPerson = new SourcePersonRequest
    {
      PersonBaseData = new RequestPersonBaseData
      {
        PersonalNumber = personRequest.PrimaryId!,
        FirstName = personRequest.FirstName!,
        LastName = personRequest.LastName!,
        PhoneNumber = personRequest!.Phone!,
        // Hierarchy = personRequest.Department! Needs to find the FK logic to assign this.
      },
      PersonTenantFreeFields = new PersonTenantFreeFields
      {
        Text1 = personRequest.Company!,
        Text2 = personRequest.PersonNumber!,
        Text3 = personRequest.PersonType!
      }
    };

    try
    {
      var personToEdit = await _context.PersonNumberLink.FirstOrDefaultAsync(x => x.EmployeeNumber == personalNumber);
      if (personToEdit == null)
      {
        var objectResult = await _exosService.GetSource($"{_url}{_personUrlStart}{personalNumber}{_personUrlEnd}");
        var person = JsonConvert.DeserializeObject<Root>(objectResult.ToString())!.Value.FirstOrDefault();
        personToEdit = new PersonNumberDB
        {
          EmployeeNumber = person!.PersonBaseData.PersonalNumber,
          PersonalId = person.PersonBaseData.PersonId
        };
        _context.PersonNumberLink.Add(personToEdit);
        await _context.SaveChangesAsync();
      }
      var response = await _client.PostAsync($"{_url}/api/v1.0/persons/{personToEdit.PersonalId}/update?ignoreBlacklist=false", ByteMaker(UpdatedPerson));

      if (!String.IsNullOrEmpty(personRequest.PinCode))
      {
        // When pin decoder is completed this line needs the same modification as in POST.
        await _client.PostAsync($"{_url}/api/v1.0/persons/{personToEdit!.PersonalId}/setPin", ByteMaker("" /*PinDecoder(personRequest.PinCode)*/));
      }
      return Ok(personalNumber);
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }
  

  [HttpDelete("{personalNumber}")]
  public async Task<IActionResult> DeletePerson(string personalNumber)
  {
    await GetPerson(personalNumber);
    var personalNumberId = await _context.PersonNumberLink.FirstOrDefaultAsync(x => x.EmployeeNumber == personalNumber);
    if (String.IsNullOrEmpty(personalNumberId!.PersonalId)) return NotFound();

    var response = await _client.PostAsync($"{_url}{_deleteUrl}{personalNumberId.PersonalId}/delete?checkOnly=false", null);

    _context.PersonNumberLink.Remove(personalNumberId);
    await _context.SaveChangesAsync();

    return !response.IsSuccessStatusCode ? StatusCode(500) : NoContent();
  }
}
