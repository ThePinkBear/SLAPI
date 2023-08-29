using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Test.Models;

internal class PersonsRepository
{
  private readonly HttpClient _client;
  private readonly string _url;
  private readonly string _accessPointUrl;
  private readonly string _scheduleUrl;

  internal PersonsRepository(IHttpClientFactory client, IConfiguration config)
  {
    _client = client.CreateClient("ExosClientDev");
    _url = config.GetValue<string>("ExosUrl") ?? throw new Exception("No url found");
    _accessPointUrl = config.GetValue<string>("Url:AccessPoint") ?? throw new Exception("No accesspoint url found");
    _scheduleUrl = config.GetValue<string>("Url:Schedule") ?? throw new Exception("No schedule url found");
  }

  internal async Task<AccessPointResponse?> GetAccessPoint(string id)
  {
    var accessPoints =
        from accesspoint in await GetListFromExos<AccessPoint>(_accessPointUrl!)
        select new AccessPointResponse
        {
          AccessPointId = accesspoint.Id,
          Address = accesspoint.Address,
          Description = accesspoint.AccessPointId
        };
    
    if (accessPoints == null) throw new Exception("No accesspoint found");
  
    return accessPoints
              .Where(x => x.AccessPointId == id)
              .Select(x => x).FirstOrDefault();
  }
  internal async Task<Schedule> GetSchedule(string id)
  {
    var schedules = await GetListFromExos<Schedule>(_scheduleUrl!);
    var schedule = schedules.FirstOrDefault(x => x.TimeZoneId == id);

    if (schedule == null) throw new Exception("No schedule found");
    
    return schedule;
  }

  private Task<List<T>> GetListFromExos<T>(string url)
  {
    var response = _client.GetAsync($"{_url}{url}").Result;
    var objectResult = JObject.Parse(response.Content.ReadAsStringAsync().Result);
    var list = JsonConvert.DeserializeObject<List<T>>(objectResult["value"]!.ToString());

    if (list == null) throw new Exception("No list found");
    
    return Task.FromResult<List<T>>(list);
  }
}