using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddDbContext<PersonsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PersonsContext") ?? throw new InvalidOperationException("Connection string 'PersonsContext' not found.")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpClient("ExosClientDev").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
              ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
              // UseDefaultCredentials = false,
              // Credentials = System.Net.CredentialCache.DefaultCredentials,
              // AllowAutoRedirect = true
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
