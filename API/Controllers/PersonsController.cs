using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Test.Models;
using System.Net.Http.Headers;

namespace SLAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PersonsController : ControllerBase
  {
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
    }

    [HttpGet]
    public async Task<ActionResult<PersonResponse>> GetPerson(string personalNumber)
    {
      var response = await _client.GetAsync($"{_url}{_personUrl1}{personalNumber}{_personUrl2}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());

      try
      {
        var person = JsonConvert.DeserializeObject<ExosPerson>(objectResult["value"]![0]!.ToString());
        var personResponse = new PersonResponse
        {
          PersonId = person!.PersonBaseData.PersonId,
          PersonalNumber = person.PersonBaseData.PersonalNumber,
          FirstName = person.PersonBaseData.FirstName,
          LastName = person.PersonBaseData.LastName
        };

        return personResponse == null ? NotFound() : Ok(personResponse);
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound();
      }
    }


    [HttpPut("{personalNumber}")]
    public async Task<IActionResult> PutPerson(string personalNumber, PersonRequest person)
    {
      var personToEdit = await GetPerson(personalNumber);
      if (personToEdit.Result is NotFoundResult) return NotFound();

      var result = ((OkObjectResult)personToEdit.Result!).Value as PersonResponse;

      var UpdatedPerson = new ExosPerson
      {
        PersonBaseData = new Person
        {
          PersonId = result!.PersonId,
          PersonalNumber = person.PersonalNumber,
          FirstName = person.FirstName,
          LastName = person.LastName,
          // Department = person.Department,
          // PinCode = person.PinCode
        }
      };
      var myContent = JsonConvert.SerializeObject(UpdatedPerson);
      var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
      var byteContent = new ByteArrayContent(buffer);
      byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

      var updateResult = await _client.PostAsync($"{_url}/v1.0/persons/{result!.PersonId}/update?ignoreBlacklist=true", byteContent);

      return !updateResult.IsSuccessStatusCode ? StatusCode(500) : NoContent() ;
    }

    [HttpPost]
    public async Task<ActionResult<ExosPersonResponse>> PostPerson(PersonRequest person)
    {
      var createdPerson = new Person
      {
        PersonId = Guid.NewGuid().ToString(),
        PersonalNumber = person.PersonalNumber,
        FirstName = person.FirstName,
        LastName = person.LastName,
        // Department = person.Department,
        // PinCode = person.PinCode
      };
      var exosPerson = new ExosPersonResponse
      {
        PersonBaseData = createdPerson
      };
      var myContent = JsonConvert.SerializeObject(exosPerson);
      var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
      var byteContent = new ByteArrayContent(buffer);
      byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

      await _client.PostAsync($"{_url}/api/v1.0/persons/create", byteContent);

      return Ok(exosPerson);

    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(string id)
    {
      var person = await GetPerson(id);
      if (person.Result is NotFoundResult) return NotFound();

      var result = ((OkObjectResult)person.Result!).Value as PersonResponse;

      var response = await _client.PostAsync($"{_url}{_deleteUrl}{result!.PersonId}/delete?checkOnly=false", null);


      return !response.IsSuccessStatusCode ? StatusCode(500) : NoContent();
    }
  }
}
