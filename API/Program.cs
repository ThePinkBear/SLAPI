using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PersonsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PersonsContext") ?? throw new InvalidOperationException("Connection string 'PersonsContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
