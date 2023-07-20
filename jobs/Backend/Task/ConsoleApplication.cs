using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application.Configurations;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Types;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

public class ConsoleApplication : BackgroundService
{
    private readonly IExchangeRateProviderService _exchangeRateProviderService;
    private readonly IEnumerable<Currency> _currencies;

    public ConsoleApplication(IExchangeRateProviderService exchangeRateProviderService, IOptions<AppConfigurations> appConfigurations)
    {
        _exchangeRateProviderService = exchangeRateProviderService;
        _currencies = appConfigurations.Value.Currencies;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var rates = _exchangeRateProviderService.GetExchangeRates(_currencies);

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