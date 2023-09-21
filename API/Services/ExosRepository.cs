using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ExosRepository
{
  public async Task<List<T>> GetExos<T>(HttpClient client, string url, string objectDepth, string objectDepth2)
  {
    var response = await client.GetAsync(url);
    var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
    return JsonConvert.DeserializeObject<List<T>>(objectResult[objectDepth]![objectDepth2]!.ToString())!;
  }

  public async Task<List<T>> GetExos<T>(HttpClient client, string url, string objectDepth)
  {
    var response = await client.GetAsync(url);
    var objectResult = JObject.Parse(await response.Content.ReadAsStringAsync());
    return JsonConvert.DeserializeObject<List<T>>(objectResult[objectDepth]!.ToString())!;
  }

  public async Task<JObject> GetExos(HttpClient client, string url)
  {
    var response = await client.GetAsync(url);
    return JObject.Parse(await response.Content.ReadAsStringAsync());
  }

}