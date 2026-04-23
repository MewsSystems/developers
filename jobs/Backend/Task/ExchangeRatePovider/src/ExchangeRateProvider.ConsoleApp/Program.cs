using ExchangeRateProvider.Application;
using ExchangeRateProvider.ConsoleApp;
using ExchangeRateProvider.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddHostedService<ExchangeRateUpdaterWorker>();

using var host = builder.Build();
await host.RunAsync();
