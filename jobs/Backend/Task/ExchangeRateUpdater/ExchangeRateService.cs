using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

public class ExchangeRateService : IHostedService
{
    private readonly IHostApplicationLifetime hostApplicationLifetime;
    private readonly IExchangeRateProvider exchangeRateProvider;
    private readonly ExchangeRateOptions exchangeRateOptions;

    public ExchangeRateService(IHostApplicationLifetime hostApplicationLifetime, IExchangeRateProvider exchangeRateProvider, IOptions<ExchangeRateOptions> inputOptions)
    {
        this.hostApplicationLifetime = hostApplicationLifetime;
        this.exchangeRateProvider = exchangeRateProvider;
        exchangeRateOptions = inputOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var rates = await exchangeRateProvider.RetrieveExchangeRatesAsync(exchangeRateOptions.RequestedCurrencies, CancellationToken.None);

            Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
            foreach (var rate in rates)
            {
                Console.WriteLine($"{rate.SourceCurrency.Code},{rate.TargetCurrency.Code},{rate.Value.ToString(CultureInfo.InvariantCulture)}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        hostApplicationLifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}