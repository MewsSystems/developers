using ExchangeRateUpdater.Api.Services;
using ExchangeRateUpdater.Core;
using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using ExchangeRateUpdater.Core.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
builder.Services.Configure<MemoryCacheOptions>(options =>
{
    var cacheSettings = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<CacheSettings>>().Value;
    options.SizeLimit = cacheSettings.SizeLimit;
    options.CompactionPercentage = cacheSettings.CompactionPercentage;
    options.ExpirationScanFrequency = cacheSettings.ExpirationScanFrequency;
});


builder.Services.AddExchangeRateCoreDependencies(builder.Configuration);
builder.Services.AddSingleton<IExchangeRateCache, ApiExchangeRateCache>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }


