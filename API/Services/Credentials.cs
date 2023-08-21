using System.Text;
using Microsoft.Win32;

public class Credentials
{
  public string Value { get; set; } ="";
  RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Exos", true);
  
  public Credentials(IConfiguration config)
  {
    Value = Convert.ToBase64String
      (
        Encoding.ASCII
        // .GetBytes($"{config.GetValue<string>("User:Name")}:{config.GetValue<string>("User:Password")}")
        .GetBytes($"{config.GetValue<string>("User:Name")}:{key.GetValue("Password")}")
      );
  }
}