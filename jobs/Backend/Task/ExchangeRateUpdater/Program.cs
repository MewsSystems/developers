using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Services;
using System;
using System.Linq;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    // Only for demonstration purposes, production would use a proper cache implementation: Redis, SQL Server, etc.
    .AddDistributedMemoryCache()
    .AddLogging(loggingBuilder =>
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        loggingBuilder.AddSerilog(logger);
    })
    .AddServices(builder.Configuration);

using var host = builder.Build();

using var scope = host.Services.CreateScope();
var currencies = args.Select(arg => arg.Trim())
    .Where(arg => !string.IsNullOrWhiteSpace(arg))
    .Select(arg => new Currency(arg))
    .ToList();

var provider = scope.ServiceProvider.GetService<IExchangeRateProvider>();
var rates = await provider.GetExchangeRates(currencies);

Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
foreach (var rate in rates)
{
    Console.WriteLine(rate.ToString());
}
