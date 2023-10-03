using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

public static class HelperMethods
{
  public static ByteArrayContent ByteMaker<T>(T epr)
  {
      var byteContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(epr)));
      byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      return byteContent;
  }
  public static string IsChanged(string? changed, string? original)
  {
    return String.IsNullOrEmpty(changed) ? original: changed;
  }
}