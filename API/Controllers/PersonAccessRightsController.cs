using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Test.Models;

namespace SLAPI.Controllers
{
  [Route("api/r/[controller]")]
  [ApiController]
  public class PersonController : ControllerBase
  {
    private readonly HttpClient _client;
    private readonly string? _url;
    private readonly string? _personUrl;

    public PersonController(IHttpClientFactory client, IConfiguration config)
    {
      _client = client.CreateClient("ExosClientDev");
      _url = config.GetValue<string>("ExosUrl");
      _personUrl = config.GetValue<string>("Url:Person");
    }

    [HttpGet]
    public async Task<ActionResult<List<AccessRightResponse>>> GetPerson(string? personId)
    {
      // todo Make sure r/person can use this to get a list of access rights for a person

      var response = await _client.GetAsync($"{_url}{_personUrl}");
      var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
      var people = JsonConvert.DeserializeObject<List<Person>>(objectResult["value"]!.ToString());
      var person = people?.Where(x => x.PrimaryId == personId);


      if (!String.IsNullOrEmpty(personId))
      { 
        // var result = personResponse
        //                 .Where(x => x.PersonId == personId)
        //                 .Select(x => x).FirstOrDefault();
        // return result == null 
        //                   ? NotFound()
        //                   : Ok(result);                                        
      }

      return Ok();
    }


  }
}
