using System;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
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

var currencies = new[]
{
    new Currency("USD"),
    new Currency("EUR"),
    new Currency("CZK"),
    new Currency("JPY"),
    new Currency("KES"),
    new Currency("RUB"),
    new Currency("THB"),
    new Currency("TRY"),
    new Currency("XYZ")
};

using (var scope = host.Services.CreateScope())
{
    var provider = scope.ServiceProvider.GetService<IExchangeRateProvider>();
    var rates = await provider.GetExchangeRates(currencies);

    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
    foreach (var rate in rates)
    {
        Console.WriteLine(rate.ToString());
    }
}

await host.RunAsync();