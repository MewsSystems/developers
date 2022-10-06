using Mews.CurrencyExchange.Domain.Models;
using Mews.CurrencyExchange.Providers.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mews.CurrencyExchange.Console
{
    internal class CurrencyExchangeApp : IHostedService
    {
        private readonly ICurrencyExchangeProvider _currencyExchangeProvider;
        private readonly ILogger _logger;

        private static IEnumerable<Currency> currencies = new[] {
            new Currency("USD"), new Currency("EUR"), new Currency("CZK"),
            new Currency("JPY"), new Currency("KES"), new Currency("RUB"),
            new Currency("THB"), new Currency("TRY"), new Currency("XYZ")
        };

        public CurrencyExchangeApp(ICurrencyExchangeProvider currencyExchangeProvider, ILogger<CurrencyExchangeApp> logger)
        {
            _currencyExchangeProvider = currencyExchangeProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, "Starting currency exchange app");

            await RetrieveAndOutputExchangeRates();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            _logger.Log(LogLevel.Information, "Stopping currency exchange app");
        }

        private async Task RetrieveAndOutputExchangeRates()
        {
            try
            {
                var rates = await _currencyExchangeProvider.GetExchangeRates(currencies);

                System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    System.Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            System.Console.ReadLine();
        }
    }
}
