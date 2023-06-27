using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Newtonsoft.Json;
using System.Net;

namespace TestAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PersonsController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;

    public PersonsController(HttpClient client, IConfiguration config)
    {
      _client = client;
      _url = config.GetValue<string>("ExosUrl");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(string id)
    {
      var persons = await _client.GetFromJsonAsync<List<Person>>($"{_url}/v1.0/persons");

      var person = persons == null ? null : persons.FirstOrDefault(x => x.PersonId == id);

      return person == null ? NotFound() : Ok(person);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(string id, PersonRequest person)
    {
      if (GetPerson(id) == null) return BadRequest();

      var result = await _client.PutAsJsonAsync($"{_url}/v1.0/persons/{id}/update", person);

      return !result.IsSuccessStatusCode ? StatusCode(500) : NoContent() ;
    }

    [HttpPost]
    public async Task<ActionResult<Person>> PostPerson(PersonRequest person)
    {
      var createdPerson = new Person
      {
        PersonId = Guid.NewGuid().ToString(),
        PrimaryId = person.PrimaryId,
        FirstName = person.FirstName,
        LastName = person.LastName,
        Department = person.Department,
        PinCode = person.PinCode
      };

      /// <summary>
      /// This is what you get back from Exos.
      /// {
      ///   Value: {  
      ///     "PersonId": "string"
      ///   },
      ///   "TimeElapsed": 2319
      /// }
      /// </summary>
      var response = await _client.PostAsJsonAsync($"{_url}/v1.0/persons/create", createdPerson);

      if (!response.IsSuccessStatusCode)
      {
        return response.StatusCode switch
        {
          HttpStatusCode.BadRequest => BadRequest(),
          HttpStatusCode.Unauthorized => Unauthorized(),
          HttpStatusCode.Forbidden => Forbid(),
          HttpStatusCode.NotFound => NotFound(),
          _ => StatusCode(500)
        };
      }

      return CreatedAtRoute("GetPerson", new { id = createdPerson.PrimaryId }, person);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(string id)
    {
      if (GetPerson(id) == null) return NotFound();

      var response = await _client.PostAsync($"{_url}/v1.0/persons/{id}/delete", null);

      return response.IsSuccessStatusCode ? NoContent() : StatusCode(500);
    }
  }
}
