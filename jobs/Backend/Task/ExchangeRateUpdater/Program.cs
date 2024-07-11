using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater;
using ExchangeRateUpdater.ExchangeApis;
using ExchangeRateUpdater.ExchangeApis.CnbApi;
using ExchangeRateUpdater.RateProvider;
using ExchangeRateUpdater.Extensions.Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient<IExchangeApi, CnbApi>((p, c) => {
    var baseUrl = p.GetRequiredService<IConfiguration>().GetSection("cnbApiUrl").Value;
    c.BaseAddress = new Uri(baseUrl);
    c.Timeout = TimeSpan.FromSeconds(60); // Timeout for all attempts combined.
}).AddRetryWithExponentialWaitPolicy(
    attempts: 3,
    waitTime: TimeSpan.FromSeconds(2),
    attemptTimeout: TimeSpan.FromSeconds(5));

builder.Services.AddTransient<ExchangeRateProvider>();
builder.Services.AddTransient<TestService>();
builder.Configuration.AddJsonFile("appsettings.json");

using var host = builder.Build();
await host.StartAsync();

// Application logic start
var app = host.Services.GetRequiredService<TestService>();
var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
await app.ExecuteAsync(lifetime.ApplicationStopping);
// Application logic end

await host.StopAsync();
