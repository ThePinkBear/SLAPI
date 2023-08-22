public class AuthInterceptor
{
  private readonly RequestDelegate _next;
  public readonly string _key;
  public readonly HttpClient _client;
  public AuthInterceptor(RequestDelegate next)
  {
    var c = new Credentials();
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