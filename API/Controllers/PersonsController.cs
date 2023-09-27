using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.Models;
using static ByteContentService;

namespace SLAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PersonsController : ControllerBase
  {
    private readonly ExosRepository _exosService;
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _personUrl1;
    private readonly string? _personUrl2;
    private readonly string? _deleteUrl;

    public PersonsController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _personUrl1 = config.GetValue<string>("Url:rPersonStart");
      _personUrl2 = config.GetValue<string>("Url:rPersonEnd");
      _deleteUrl = config.GetValue<string>("Url:DeletePerson");
      _exosService = new ExosRepository();
    }

    [HttpGet]
    public async Task<ActionResult<BetsyPersonResponse>> GetPerson(string personalNumber)
    {
      var objectResult = await _exosService.GetExos(_client, $"{_url}{_personUrl1}{personalNumber}{_personUrl2}");

      try
      {
        var person = JsonConvert.DeserializeObject<ExosPersonResponse>(objectResult["value"]![0]!.ToString());
        var personResponse = new BetsyPersonResponse
        {
          PersonId = person!.PersonBaseData.PersonId,
          PersonalNumber = person.PersonBaseData.PersonalNumber,
          FirstName = person.PersonBaseData.FirstName,
          LastName = person.PersonBaseData.LastName,
          Department = person.PersonTenantFreeFields.Department,
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
    public async Task<IActionResult> PutPerson(string personalNumber, BetsyPersonRequest person)
    {
      var personToEdit = await GetPerson(personalNumber);
      if (personToEdit.Result is NotFoundResult) return NotFound();

      var result = ((OkObjectResult)personToEdit.Result!).Value as Person;

      var UpdatedPerson = new ExosPersonRequest
      {
        PersonBaseData = new Person
        {
          PersonId = result!.PersonId,
          PersonalNumber = person.PersonalNumber,
          FirstName = person.FirstName,
          LastName = person.LastName,
        },
        PersonAccessControlData = new ExosAccessControl
        {
          PinCode = person.PinCode
        },
        PersonTenantFreeFields = new PersonTenantFreeFields
        {
          Department = person.Department
        }
      };

      // if (person.PinCode != null) 
      // {
      //   await _client.PostAsync($"{_url}/api/v1.0/persons/{result.PersonId}/setPin", ByteMaker(new PinRequest { PinCode = person.PinCode }));
      // }

      try
      {
        await _client.PostAsync($"{_url}/api/v1.0/persons/{result!.PersonId}/update?ignoreBlacklist=false", ByteMaker(UpdatedPerson));
        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(409, ex.Message);
      }
    }

    [HttpPost]
    public async Task<ActionResult> PostPerson(BetsyPersonRequest person)
    {
      var createdPerson = new Person
      {
        PersonId = Guid.NewGuid().ToString(),
        PersonalNumber = person.PersonalNumber,
        FirstName = person.FirstName,
        LastName = person.LastName,
        PinCode = person.PinCode
      };
      var exosPerson = new ExosPersonRequest
      {
        PersonBaseData = createdPerson,
        PersonTenantFreeFields = new PersonTenantFreeFields
        {
          Department = person.Department
        }
      };
      try
      {
        await _client.PostAsync($"{_url}/api/v1.0/persons/create", ByteMaker(exosPerson));

        return Ok();
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
}
