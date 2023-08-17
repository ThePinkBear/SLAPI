public class Interceptor
{
  private readonly RequestDelegate _next;
  public readonly string _key;
  public readonly HttpClient _client;
  public Interceptor(RequestDelegate next, IConfiguration config)
  {
    var c = new Credentials(config);
    _client = new HttpClient();
    _next = next;
    _key = c.Value;
  }

  public async Task Invoke(HttpContext context)
  {
    _client.DefaultRequestHeaders.Authorization = new System.Net.Http
              .Headers.AuthenticationHeaderValue("Basic", _key);
    await _next(context);
  }
}