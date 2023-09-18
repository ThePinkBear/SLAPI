using System.Text;
using Microsoft.Win32;

public class CredentialsService
{
  public string Value { get; set; } ="";
  RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\dormakaba", true);
  
  public CredentialsService()
  {
    Value = Convert.ToBase64String
      (
        Encoding.ASCII
        .GetBytes($"MyApiKey:{key?.GetValue("SL_API_Token")}")
      );
  }
}