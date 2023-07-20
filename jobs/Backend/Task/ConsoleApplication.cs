using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater;

public class ConsoleApplication : BackgroundService
{
    private readonly IExchangeRateProviderService _exchangeRateProviderService;
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

    public ConsoleApplication(IExchangeRateProviderService exchangeRateProviderService)
    {
        _exchangeRateProviderService = exchangeRateProviderService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var rates = _exchangeRateProviderService.GetExchangeRates(currencies);

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

        await Task.Delay(1000, stoppingToken);
    }
}