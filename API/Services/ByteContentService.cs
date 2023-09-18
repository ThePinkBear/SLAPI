using System.Net.Http.Headers;
using Newtonsoft.Json;

public static class ByteContentService
{
  public static ByteArrayContent ByteMaker<T>(T epr)
  {
      var byteContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(epr)));
      byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      return byteContent;
  }
}