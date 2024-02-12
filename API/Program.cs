var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration, enables writing the console logging to a file.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File(@$"{builder.Configuration.GetValue<string>("Settings:LogFIle")!}", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Comment out this line to write logs to console instead of file
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
// Links to the appsettings.json
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// This is where the exos calls are dressed with the required headers
builder.Services.AddHttpClient("ExosClientDev", c =>
{
  c.DefaultRequestHeaders.Add("Accept", "application/json");
  c.DefaultRequestHeaders.Add("Authorization", $"Basic {new CredentialsService(builder.Configuration).Value}");
}).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
  ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
});
// Database hooks in here
builder.Services.AddDbContext<AccessContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("Exos"));
});

// This is a brute forced bypass of CORS, not allowed in cloud.
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll", builder =>
  {
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
  });
});

var app = builder.Build();

// Https redirection needs to be added here if it's wanted, we romeved it together when it blocked access in UAT/SitSat

app.UseSwagger(); // Comment out to remove swagger.
app.UseSwaggerUI();

app.UseCors("AllowAll"); // Comment out to remove CORS bypass
app.UseAuthorization();
app.MapControllers();

app.Run();

