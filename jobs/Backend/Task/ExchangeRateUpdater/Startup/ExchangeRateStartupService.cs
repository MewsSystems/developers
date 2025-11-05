using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater.Startup;

public class ExchangeRateStartupService(IExchangeRateProvider provider, IConfiguration configuration) : BackgroundService
{
    private static readonly string[] DefaultCurrencyCodes =
    [
        "USD", "EUR", "CZK", "JPY", "KES", "RUB", "THB", "TRY", "XYZ"
    ];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var codes = configuration.GetSection("Currencies").Get<string[]>() ?? DefaultCurrencyCodes;
            var currencies = codes
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim().ToUpperInvariant())
                .Distinct()
                .Select(c => new Currency(c))
                .ToArray();

            var rates = await provider.GetExchangeRates(currencies);

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
    }
}
