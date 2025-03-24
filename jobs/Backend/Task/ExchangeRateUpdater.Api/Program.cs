using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Handlers;
using ExchangeRateUpdater.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogging()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379";

builder.Services
    .AddOpenTelemetry(builder.Environment.ApplicationName)
    .AddDistributedRedisCache(builder.Environment.ApplicationName, redisConnectionString)
    .AddValidators()
    .AddMediatr()
    .AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOutputCache();

app.MapPrometheusScrapingEndpoint();

app.MapGet("/exchange-rates", ExchangeRatesHandler.GetExchangeRates)
    .CacheOutput(x => x.Expire(TimeSpan.FromMinutes(5)))
    .WithName("GetExchangeRates")
    .WithOpenApi();

app.Run();