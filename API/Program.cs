global using Microsoft.EntityFrameworkCore;


string path = @"C:\inetpub\wwwroot\SLAPI\LOGS\Output.txt";
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File(path, rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add services to the container.
builder.Host.UseSerilog(); // Use Serilog as the logging framework
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpClient("ExosClientDev", c =>
{
  c.DefaultRequestHeaders.Add("Accept", "application/json");
  c.DefaultRequestHeaders.Add("Authorization", $"Basic {new CredentialsService().Value}");
}).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
  ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
});
builder.Services.AddDbContext<AccessContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("Exos"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

