using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class AppService : IHostedService
    {
        private readonly ILogger<AppService> _logger;
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly IEnumerable<string> _supportedCurrencyCodes;

        public AppService(IExchangeRateProvider exchangeRateProvider, IOptions<CurrencySettings> currencyOptions, ILogger<AppService> logger)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _supportedCurrencyCodes = currencyOptions.Value.SupportedCurrencies;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting application.");
            await DisplayExchangeRatesAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping application.");
        }

        public async Task DisplayExchangeRatesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var currencies = _supportedCurrencyCodes.Select(c => new Currency(c));

                var rates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies, date: DateTime.UtcNow, cancellationToken: cancellationToken);

                if (!rates.Any())
                {
                    Console.WriteLine("There are no rates to display");
                    return;
                }

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
}
