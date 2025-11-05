using ConfigurationLayer.Interface;
using ConfigurationLayer.Option;
using ConfigurationLayer.Service;

using DataLayer;
var builder = WebApplication.CreateBuilder(args);

// Register configuration options from appsettings.json
builder.Services.Configure<SystemConfigurationOptions>(
    builder.Configuration.GetSection("SystemConfiguration"));

builder.Services.Configure<ExchangeRateProvidersOptions>(
    builder.Configuration.GetSection("ExchangeRateProviders"));

builder.Services.AddDataLayer(builder.Configuration);

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
builder.Services.AddSingleton<IProviderConfigurationService, ProviderConfigurationService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
