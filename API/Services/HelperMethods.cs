public static class HelperMethods
{
  public static HttpContent ByteMaker<T>(T epr)
  {
    HttpContent byteContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(epr)));
    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    return byteContent;
  }

  public static string IsChanged(string? changed, string? original)
  {
    return String.IsNullOrEmpty(original) switch
    {
      true => !String.IsNullOrEmpty(changed) ? changed : "",
      false => !String.IsNullOrEmpty(changed) ? changed : original
    };
  }

  public static async Task<List<AccessRightMatcher>> GetAccessRights(SourceRepository repo, string url, string accessRightUrl)
  {
    var accessRightIds = await repo.GetExos<SourceAccessRightResponse>($"{url}{accessRightUrl}", "value");

    Dictionary<string, string> ArIdTzId = new();

    foreach (var ar in accessRightIds)
    {
      var ar2 = await repo.GetExos<SourceScheduleResponse>($"{url}/api/v1.0/timeZones?accessRightId={ar.AccessRightId}&%24count=true&%24top=4", "value");

      ArIdTzId.Add(ar.DisplayName, ar2[0].TimeZoneId);
    }

    List<AccessRightMatcher> accessRights = new();

    foreach (var ar in accessRightIds)
    {
      accessRights.Add(new AccessRightMatcher
      {
        rid = ar.AccessRightId,
        aid = ar.DisplayName,
        sid = ArIdTzId[ar.DisplayName]
      });
    }
    return accessRights;
  }
  public static string PinDecoder(string pinCode)
  {
    /// <summary>
    /// This is where the logic to decrypt the picode will go
    /// </summary>
    return pinCode;
  }
}