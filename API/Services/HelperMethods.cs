using System.Diagnostics;
using System.Net.Http.Headers;

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
    return String.IsNullOrEmpty(original) switch
    {
      true => !String.IsNullOrEmpty(changed) ? changed : "",
      false => !String.IsNullOrEmpty(changed) ? changed : original
    };

  }
  public static void RunMigration()
  {
    ProcessStartInfo startInfo = new ProcessStartInfo
    {
      FileName = "dotnet",
      Arguments = "ef database update",
      RedirectStandardOutput = true,
      UseShellExecute = false,
      CreateNoWindow = true,
    };

    using (Process process = new Process { StartInfo = startInfo })
    {
      process.Start();

      // Read the output to console (optional)
      string result = process.StandardOutput.ReadToEnd();
      Console.WriteLine(result);

      process.WaitForExit();
    }
  }
}