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

    public PersonsController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _personUrl1 = config.GetValue<string>("Url:rPersonStart");
      _personUrl2 = config.GetValue<string>("Url:rPersonEnd");
    }

    [HttpGet]
    public async Task<ActionResult<List<PersonResponse>>> GetPerson(string? personalNumber)
    {
      var response = await _client.GetAsync($"{_url}{_personUrl1}{personalNumber}{_personUrl2}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var person = JsonConvert.DeserializeObject<Person>(objectResult["value"]![0].ToString());

      var personResponse = new PersonResponse
      {
        PersonalNumber = person.PersonalNumber,
        FirstName = person.FirstName,
        LastName = person.LastName
      };

      return personResponse == null ? NotFound() : Ok(personResponse);
    }


    // [HttpPut("{id}")]
    // public async Task<IActionResult> PutPerson(string id, PersonRequest person)
    // {
    //   if ((await GetPerson(id)).Result is NotFoundResult) return NotFound();

    //   var result = await _client.PutAsJsonAsync($"{_url}/v1.0/persons/{id}/update", person);

    //   return !result.IsSuccessStatusCode ? StatusCode(500) : NoContent() ;
    // }

    [HttpPost]
    public async Task<ActionResult<ExosPerson>> PostPerson(PersonRequest person)
    {
      var createdPerson = new Person
      {
        PersonId = Guid.NewGuid().ToString(),
        PersonalNumber = person.PersonalNumber, // Personal Number supplied by SL.
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
      if ((await GetPerson(id)).Result is NotFoundResult) return BadRequest();

      var response = await _client.PostAsync($"{_url}/v1.0/persons/{id}/delete", null);

      return response.IsSuccessStatusCode ? NoContent() : StatusCode(500);
    }
  }
}
