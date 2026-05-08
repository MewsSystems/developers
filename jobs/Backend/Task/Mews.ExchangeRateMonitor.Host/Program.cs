using Mews.ExchangeRateMonitor.Common.API;
using Mews.ExchangeRateMonitor.Common.API.Extensions;
using Mews.ExchangeRateMonitor.Common.Infrastructure;
using Mews.ExchangeRateMonitor.ExchangeRate.Features;

var builder = WebApplication.CreateBuilder(args);

builder.AddCoreHostLogging();
builder.Services.AddCoreWebApiInfrastructure();
builder.Services.AddCoreInfrastructure();
builder.Services.AddExchangeRatesModule(builder.Configuration);

var app = builder.Build();

app.UseSwaggerExt();
app.UseAppRateLimiter();
app.MapApiEndpoints();
app.Run();
