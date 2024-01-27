using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateUpdaterJob : BackgroundService
{
    private readonly IHostApplicationLifetime _appLifetime;

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

    public ExchangeRateUpdaterJob(IHostApplicationLifetime appLifetime)
    {
        _appLifetime = appLifetime;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var provider = new ExchangeRateProvider();
            var rates = provider.GetExchangeRates(currencies);

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
        _appLifetime.StopApplication();
        
        return Task.CompletedTask;
    }
}
