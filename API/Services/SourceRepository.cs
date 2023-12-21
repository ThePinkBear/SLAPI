using System.Data;

public class SourceRepository
{
  private readonly AccessContext _context;
  private readonly HttpClient _client;

  public SourceRepository(HttpClient client, AccessContext context)
  {
    _client = client;
    _context = context;
  }

  public async Task<List<T>> GetExos<T>(string url, string objectDepth, string objectDepth2)
  {
    var response = await _client.GetAsync(url);
    var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
    return JsonConvert.DeserializeObject<List<T>>(objectResult[objectDepth]![objectDepth2]!.ToString())!;
  }

  public async Task<List<T>> GetExos<T>(string url, string objectDepth)
  {
    var response = await _client.GetAsync(url);
    var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
    return JsonConvert.DeserializeObject<List<T>>(objectResult[objectDepth]!.ToString())!;
  }

  public async Task<JObject> GetSource(string url)
  {
    var response = await _client.GetAsync(url);
    return JObject.Parse(await response.Content.ReadAsStringAsync());
  }
}