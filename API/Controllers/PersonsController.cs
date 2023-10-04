namespace SLAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
  private readonly ExosRepository _exosService;
  private readonly HttpClient _client;
  private readonly string? _url;
  private readonly string? _personUrlStart;
  private readonly string? _personUrlEnd;
  private readonly string? _deleteUrl;
  private readonly string? _createUrl;

  public PersonsController(IHttpClientFactory client, IConfiguration config)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl");
    _personUrlStart = config.GetValue<string>("Url:GetOnePersonStart");
    _personUrlEnd = config.GetValue<string>("Url:GetOnePersonEnd");
    _deleteUrl = config.GetValue<string>("Url:DeletePerson");
    _createUrl = config.GetValue<string>("Url:CreatePerson");
    _exosService = new ExosRepository();
  }

  [HttpGet]
  public async Task<ActionResult<BetsyPersonResponse>> GetPerson(string personalNumber)
  {
    try
    {
      var objectResult = await _exosService.GetExos(_client, $"{_url}{_personUrlStart}{personalNumber}{_personUrlEnd}");
      var person = JsonConvert.DeserializeObject<ExosPersonsResponse>(objectResult["value"]![0]!.ToString());
      var personResponse = new BetsyPersonResponse
      {
        PersonId = person!.PersonBaseData.PersonId,
        PersonalNumber = person.PersonBaseData.PersonalNumber,
        FirstName = person.PersonBaseData.FirstName,
        LastName = person.PersonBaseData.LastName,
        Department = person.PersonTenantFreeFields.Text3
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
  public async Task<IActionResult> PutPerson(string personalNumber, BetsyPersonPutRequest personRequest)
  {
    if (String.IsNullOrEmpty(personalNumber) || personRequest == null) return BadRequest();

    var personToEdit = await GetPerson(personalNumber);
    
    if (personToEdit.Result is NotFoundResult) return NotFound();

    var personToEditValues = ((OkObjectResult)personToEdit.Result!).Value as BetsyPersonResponse;

    var UpdatedPerson = new ExosPersonRequest
    {
      PersonBaseData = new PersonBaseData
      {
        PersonalNumber = IsChanged(personRequest.PersonalNumber, personToEditValues!.PersonalNumber),
        FirstName = IsChanged(personRequest.FirstName, personToEditValues.FirstName),
        LastName = IsChanged(personRequest.LastName, personToEditValues.LastName)
      }
      // PersonTenantFreeFields = new PersonTenantFreeFields
      // {
      //   Text3 = IsChanged(personRequest.Department, personToEditValues.Department)
      // }
    };



    try
    {
      var response = await _client.PostAsync($"{_url}/api/v1.0/persons/{personToEditValues!.PersonId}/update?ignoreBlacklist=false", ByteMaker(UpdatedPerson));

      if (!String.IsNullOrEmpty(personRequest.PinCode))
      {
        await _client.PostAsync($"{_url}/api/v1.0/persons/{personToEditValues!.PersonId}/setPin", ByteMaker(personRequest.PinCode));
      }
      return Ok(personRequest.PersonalNumber);
    }
    catch (Exception ex)
    {
      return StatusCode(409, ex.Message);
    }
  }

  [HttpPost]
  public async Task<ActionResult> PostPerson(BetsyPersonCreateRequest person)
  {
    var createdPerson = new PersonBaseData
    {
      PersonalNumber = person.PersonalNumber,
      FirstName = person.FirstName ?? "",
      LastName = person.LastName ?? ""
    };
    var personTenantFreeFields = new PersonTenantFreeFields
    {
      Text3 = person.Department
    };
    var exosPerson = new ExosPersonRequest
    {
      PersonBaseData = createdPerson,
      PersonTenantFreeFields = personTenantFreeFields
    };
    try
    {
      var posted = await _client.PostAsync($"{_url}{_createUrl}", ByteMaker(exosPerson));

      if (posted.IsSuccessStatusCode && !String.IsNullOrEmpty(person.PinCode))
      {
        var response = await GetPerson(person.PersonalNumber!);
        var result = ((OkObjectResult)response.Result!).Value as BetsyPersonResponse;
        await _client.PostAsync($"{_url}/api/v1.0/persons/{result!.PersonId}/setPin", ByteMaker(person.PinCode));
      }
      // Return CreatedAtRoute?
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
    var person = await GetPerson(personalNumber);
    if (person.Result is NotFoundResult) return NotFound();

    var result = ((OkObjectResult)person.Result!).Value as BetsyPersonResponse;

    var response = await _client.PostAsync($"{_url}{_deleteUrl}{result!.PersonId}/delete?checkOnly=false", null);

    return !response.IsSuccessStatusCode ? StatusCode(500) : NoContent();
  }
}
