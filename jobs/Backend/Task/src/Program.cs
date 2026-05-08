using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache(); 
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IExchangeRateSettingsResolver, ExchangeRateSettingsResolver>();
builder.Services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var app = builder.Build();

app.UseSwagger(c =>
{
    c.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
});
app.UseSwaggerUI();
app.MapGet("/", () => "Exchange Rate Updater is running!");
app.MapControllers();
app.Run();