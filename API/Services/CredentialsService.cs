using System.Text;

public class CredentialsService
{
  public string Value { get; set; } = "";
  private readonly string? _path;
  

  public CredentialsService(IConfiguration config)
  {
    _path = config.GetValue<string>("Settings:TokenPath");
    Value = Convert.ToBase64String
      (
        Encoding.ASCII.GetBytes($"MyApiKey:{File.ReadAllText(@$"{_path}", Encoding.UTF8).Trim()}")
      );
  }
}