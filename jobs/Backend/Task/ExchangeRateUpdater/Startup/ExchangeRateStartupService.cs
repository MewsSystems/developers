using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Configuration.Options;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Providers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Startup;

public class ExchangeRateStartupService(
        IExchangeRateProvider provider,
        IOptions<CurrencyOptions> options
    ) : BackgroundService
{
    private static readonly IEnumerable<Currency> DefaultCurrencies = new[]
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
    
    private readonly IEnumerable<Currency> _currencies = !options.Value.Currencies.Any() ? DefaultCurrencies : options.Value.Currencies;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var provider = new ExchangeRateProvider();
            var rates = await provider.GetExchangeRates(_currencies);

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
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