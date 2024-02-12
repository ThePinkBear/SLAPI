using System.Data;

/// <summary>
/// The overloads of GetExos method found below takes the different signatures provided in the endpoints throughout
/// the controllers and provides Deserilization objects in a more readable manner therein.
/// </summary>

public class SourceRepository
{
  private readonly HttpClient _client;

  public SourceRepository(HttpClient client, AccessContext context)
  {
    _client = client;
  }

  // Generic overload to get specific DTO type out with object depth of 2 in exos
  public async Task<List<T>> GetExos<T>(string url, string objectDepth, string objectDepth2)
  {
    var response = await _client.GetAsync(url);
    var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
    return JsonConvert.DeserializeObject<List<T>>(objectResult[objectDepth]![objectDepth2]!.ToString())!;
  }
   // Generic overload to get specific DTO type out with object depth of 1 in exos
  public async Task<List<T>> GetExos<T>(string url, string objectDepth)
  {
    var response = await _client.GetAsync(url);
    var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
    return JsonConvert.DeserializeObject<List<T>>(objectResult[objectDepth]!.ToString())!;
  }
  // Returns the Jobject where above methods are unapplicable
  public async Task<JObject> GetSource(string url)
  {
    var response = await _client.GetAsync(url);
    return JObject.Parse(await response.Content.ReadAsStringAsync());
  }
}