using System.Data;

public class ExosRepository
{
  private readonly AccessContext _context;
  private readonly HttpClient _client;

  public ExosRepository(HttpClient client, AccessContext context)
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

  public async Task<JObject> GetExos(string url)
  {
    var response = await _client.GetAsync(url);
    return JObject.Parse(await response.Content.ReadAsStringAsync());
  }
  public List<BetsyAccessRightResponse> ExosDbGetAccessRights()
  {
    var accessRights = _context.AccessRights;

    var result = from accessRight in accessRights
                 select new BetsyAccessRightResponse
                 {
                   PersonPrimaryId = accessRight.PersonPrimaryId,
                   AccessPointId = accessRight.AccessPointId,
                   ScheduleId = accessRight.ScheduleId,
                   AccessRightId = accessRight.UniqueId
                 };

    return result.ToList();
  }
  public async Task<string> ExosPostAccessRightDb(BetsyAccessRightRequest request)
  {
    var newAr = new AccessRightDbObject
    {
      UniqueId = Guid.NewGuid().ToString(),
      PersonPrimaryId = request.PersonPrimaryId,
      ScheduleId = request.TimeZoneId,
      AccessPointId = request.AccessPointId
    };

    _context.AccessRights.Add(newAr);

    await _context.SaveChangesAsync();
    return newAr.UniqueId;
  }
  public async void ExosDeletAccessRight(string id)
  {
    var accessRight = _context.AccessRights.Where(a => a.UniqueId == id).First();
    _context.AccessRights.Remove(accessRight);
    await _context.SaveChangesAsync();
  }
  public async Task<BetsyAccessRightResponse> ExosPutAccessRight(int id, BetsyAccessRightRequest request)
  {
    var accessRight = await _context.AccessRights.Where(x => x.AccessRightId == id).FirstOrDefaultAsync();
    var modification = new AccessRightDbObject
    {
      AccessPointId = IsChanged(request.AccessPointId, accessRight?.AccessPointId),
      PersonPrimaryId = IsChanged(request.PersonPrimaryId, accessRight?.PersonPrimaryId),
      ScheduleId = IsChanged(request.TimeZoneId, accessRight?.ScheduleId)
    };
    try
    {
      _context.AccessRights.Update(modification);
      await _context.SaveChangesAsync();
    }
    catch (DBConcurrencyException ex)
    {
      Console.WriteLine(ex.Message);
    }

    var response = new BetsyAccessRightResponse
    {
      AccessPointId = modification.AccessPointId,
      PersonPrimaryId = modification.PersonPrimaryId,
      ScheduleId = modification.ScheduleId
    };

    return response;
  }
}