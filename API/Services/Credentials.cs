using System.Text;

public class Credentials
{
  public string Value { get; set; } =""; 
  
  public Credentials(IConfiguration config)
  {
    Value = Convert.ToBase64String
      (
        Encoding.ASCII
        .GetBytes($"{config.GetValue<string>("User:Name")}:{config.GetValue<string>("User:Password")}")
      );
  }
}