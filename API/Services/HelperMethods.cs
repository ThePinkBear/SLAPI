public static class HelperMethods
{
  public static HttpContent ByteMaker<T>(T epr)
  {
    HttpContent byteContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(epr)));
    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    return byteContent;
  }

  /// <summary>
  /// A small helper method that checks if a string has been changed from the original and returns accordingly
  /// Might be redundant but it solved some bugs when trying to make put changes in Swagger so I've left it in 
  /// to be safe
  /// </summary>
  public static string IsChanged(string? changed, string? original)
  {
    return String.IsNullOrEmpty(original) switch
    {
      true => !String.IsNullOrEmpty(changed) ? changed : "",
      false => !String.IsNullOrEmpty(changed) ? changed : original
    };
  }

  /// <summary>
  /// Handles the link between accessRights and accesspoints and I expect it to be one of the heavier actions 
  /// in terms of the amount of calls to Exos, I expect yuo'll see if it's problematic in the logs.
  /// I haven't seen a beter way to refactor it yet though so there it is.
  /// It's placed here as it is a lot of code and would make the endpoint bloated and unreadable. 
  /// </summary>
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