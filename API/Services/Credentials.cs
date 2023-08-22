using System.Text;
using Microsoft.Win32;

public class Credentials
{
  public string Value { get; set; } ="";
  RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\dormakaba", true);
  
  public Credentials()
  {
    Value = Convert.ToBase64String
      (
        Encoding.ASCII
        .GetBytes($"MyApiKey:{key?.GetValue("SL_API_Token")}")
      );
  }
}