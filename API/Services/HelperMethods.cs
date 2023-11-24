using System.Diagnostics;
using System.Net.Http.Headers;

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
}