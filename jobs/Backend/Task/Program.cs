using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly.Extensions.Http;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace ExchangeRateUpdater;

public static class Program
{
    private static IEnumerable<Currency> currencies = new[]
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

    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient(nameof(ExchangeRateService), client =>
                {
                    client.BaseAddress = new Uri("https://api.cnb.cz/cnbapi/"); // Replace with the actual API URL
                });

                services.AddScoped<IExchangeRateService, ExchangeRateService>();
                services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
                services.AddSingleton(TimeProvider.System);
                services.AddSingleton<IMemoryCache, MemoryCache>();

                services.AddHealthChecks()
                        .AddCheck<ExchangeRateHealthCheck>("ExchangeRateAPI");

                OpenTelemetryConfiguration.ConfigureOpenTelemetry(services);
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddOpenTelemetry(options =>
                    {
                        options.IncludeScopes = true;
                        options.ParseStateValues = true;
                        options.IncludeFormattedMessage = true;
                    });
                });
            })
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.Configure(app =>
                  {
                      app.UseRouting();

                      app.UseEndpoints(endpoints =>
                      {
                          endpoints.ConfigureHealthCheckEndpoint();

                      });
                  });
              })
              .Build();

        var exchangeRateProvider = host.Services.GetRequiredService<IExchangeRateProvider>();

        try
        {
            var rates = await  exchangeRateProvider.GetExchangeRates(currencies);

            Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }
}
