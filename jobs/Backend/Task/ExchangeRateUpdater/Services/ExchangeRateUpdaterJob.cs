using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mews.Integrations.Cnb.Contracts.Configuration;
using Mews.Integrations.Cnb.Contracts.Models;
using Mews.Integrations.Cnb.Contracts.Services;
using Mews.Shared.Temporal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateUpdaterJob(
    IHostApplicationLifetime appLifetime,
    IExchangeRateProvider exchangeRateProvider,
    IClock clock,
    IOptionsSnapshot<CnbConfiguration> cnbConfiguration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var currencies = cnbConfiguration.Value.Currencies.Select(c => new Currency(c)).ToList();
        try
        {
            var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies, clock.Now, cancellationToken);

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
        appLifetime.StopApplication();
    }
}
