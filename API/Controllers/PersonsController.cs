using Microsoft.AspNetCore.Mvc;
using Test.Models;

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

      return person == null ? NotFound() : person;
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(string id, PersonCreateRequest person)
    {
      var updatedPerson = await _client.PutAsJsonAsync($"{_url}/v1.0/persons/{id}/update", person);

      //TODO: Do some verification with updatedPerson.

      return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Person>> PostPerson(PersonCreateRequest person)
    {
        var personToCreate = new Person
        {
            PersonId = Guid.NewGuid().ToString(),
            FirstName = person.FirstName,
            LastName = person.LastName,
            Department = person.Department,
            PinCode = person.PinCode
        };
        // TODO: Check what you get back from Exos.
        var createdPerson = await _client.PostAsJsonAsync($"{_url}/v1.0/persons/create", personToCreate);

        return CreatedAtAction("GetPerson", new { id = personToCreate.PersonId }, createdPerson);
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePerson(string id)
    {
        _client.DeleteAsync($"{_url}/v1.0/persons/{id}/delete");
        return NoContent();
    }
  }
}
