using ExchangeRateUpdater.Api.Services;
using ExchangeRateUpdater.Core;
using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using ExchangeRateUpdater.Core.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
var cacheSettings = builder.Configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();
builder.Services.Configure<MemoryCacheOptions>(options =>
{
    options.SizeLimit = cacheSettings.SizeLimit;
    options.CompactionPercentage = cacheSettings.CompactionPercentage;
    options.ExpirationScanFrequency = cacheSettings.ExpirationScanFrequency;
});

builder.Services.AddExchangeRateCoreDependencies(builder.Configuration);
builder.Services.AddSingleton<IExchangeRateCache, ApiExchangeRateCache>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.MapOpenApi();

app.MapGet("/", () => "Exchange Rate Updater API is running.");

app.Run();

public partial class Program { }


