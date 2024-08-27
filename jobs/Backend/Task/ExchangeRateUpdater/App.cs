using ExchangeRateUpdater.Lib.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace ExchangeRateUpdater
{
    internal class App
    {
        private readonly ILogger _logger;
        private readonly IExchangeRateProviderSettings _settings;
        private readonly IEnumerable<IExchangeRateProvider> _providers;

        public App(
            ILogger logger,
            IExchangeRateProviderSettings settings,
            IEnumerable<IExchangeRateProvider> providers
            )
        {
            _logger = logger;
            _settings = settings;
            _providers = providers;
        }

        public void Run(
            ICollection<Currency> currencies,
            bool usePipedOutput
        )
        {
            List<ExchangeRate> rates = new();

            try
            {
                // get the rates from all of the registered providers
                rates = _providers.SelectMany(pro => pro.GetExchangeRates(currencies)).ToList();

                _logger.Info($"Successfully retrieved {rates.Count()} exchange rates:");

                if (usePipedOutput)
                {
                    // if were using piped output we need to serialize and write to the console output
                    string jsonString = JsonSerializer.Serialize(new { rates }, new JsonSerializerOptions { WriteIndented = true });
                    Console.Write(jsonString);
                }
                else
                {
                    // if were not using piped output, just output the values to the console
                    foreach (var rate in rates)
                    {
                        _logger.Info(rate.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Could not retrieve exchange rates: '{e.Message}'.");
            }
        }
    }
}
