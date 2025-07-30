using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Parsers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache(); 
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IExchangeRateSettingsResolver, ExchangeRateSettingsResolver>();
builder.Services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var app = builder.Build();

app.MapControllers();
app.Run();