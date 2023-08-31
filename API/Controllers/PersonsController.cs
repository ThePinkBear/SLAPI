using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Test.Models;

namespace SLAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PersonsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _personUrl;

    public PersonsController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _personUrl = config.GetValue<string>("Url:Person");
    }

    [HttpGet]
    public async Task<ActionResult<List<PersonResponse>>> GetPerson(string? personId)
    {
      var response = await _client.GetAsync($"{_url}{_personUrl}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var people = JsonConvert.DeserializeObject<List<Person>>(objectResult["value"]!.ToString());

      var personResponse =
        from person in people
        select new PersonResponse
        {
          PersonId = person.PersonId,
          PrimaryId = person.PrimaryId,
          FirstName = person.FirstName,
          LastName = person.LastName
        };

      if (!String.IsNullOrEmpty(personId))
      {
        var personMatch = personResponse
                        .Where(x => x.PrimaryId == personId)
                        .Select(x => x).FirstOrDefault();
        return personMatch == null
                          ? NotFound()
                          : Ok(personMatch);
      }

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
        PersonId = new Random().Next(1 ,99).ToString(),/*Guid.NewGuid().ToString(),*/
        PrimaryId = person.PrimaryId, // Personal Number supplied by SL.
        FirstName = person.FirstName,
        LastName = person.LastName,
        // Department = person.Department,
        // PinCode = person.PinCode
      };
      var exosPerson = new ExosPerson
      {
        PersonCategory  = 1,
        PersonBaseData = createdPerson
      };

      var response = await _client.PostAsJsonAsync($"{_url}/api/v1.0/persons/create", exosPerson);

      if (!response.IsSuccessStatusCode) return Ok(exosPerson);
      return StatusCode(409, exosPerson);
    }


    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeletePerson(string id)
    // {
    //   if ((await GetPerson(id)).Result is NotFoundResult) return BadRequest();

    //   var response = await _client.PostAsync($"{_url}/v1.0/persons/{id}/delete", null);

    //   return response.IsSuccessStatusCode ? NoContent() : StatusCode(500);
    // }
  }
}
