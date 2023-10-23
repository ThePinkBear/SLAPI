using System.Text;

public class CredentialsService
{
  public string Value { get; set; } = "";
  
  public string key = File.ReadAllText(@"C:\Temp\exos.token", Encoding.UTF8).Trim();

  public CredentialsService()
  {
    Value = Convert.ToBase64String
      (
        Encoding.ASCII.GetBytes($"MyApiKey:{key}")
      );
  }
}