var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddHttpClient("ExosClientDev", c =>
{
  c.DefaultRequestHeaders.Add("Accept", "application/json");
  c.DefaultRequestHeaders.Add("Authorization", $"Basic {new CredentialsService(builder.Configuration).Value}");
}).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
{
  ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
});

builder.Services.AddDbContext<AccessContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("Exos"));
});


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

app.UseSwagger(); 
app.UseSwaggerUI();

app.UseCors("AllowAll"); // Comment out to remove CORS bypass
app.UseAuthorization();
app.MapControllers();

app.Run();

