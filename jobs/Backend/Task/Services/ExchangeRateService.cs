using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;

namespace ExchangeService
{
    public class ExchangeRateService
    {
        private ILogger _logger = null;
        private ExchangeRateProvider _exchangeRateProvider = null;
        private static IEnumerable<Currency> currencies = null;

        public ExchangeRateService(ILogger logger)
        {
            _logger = logger;
            _exchangeRateProvider = new ExchangeRateProvider();

            currencies = new[]
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
        }

        public async Task Execute()
        {
            try
            {
                List<ExchangeRate> filteredExchangeRates = await _exchangeRateProvider.GetExchangeRates(currencies, _logger);

                _logger.LogInformation($"Retrieved {filteredExchangeRates.Count()} after filtering");

                if (filteredExchangeRates != null && filteredExchangeRates.Count > 0)
                {
                    Console.WriteLine($"Successfully retrieved {filteredExchangeRates.Count()} exchange rates:");
                    foreach (ExchangeRate exchangeRate in filteredExchangeRates)
                    {
                        Console.WriteLine(exchangeRate.ToString());
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve Exchange Rates. Would you like to try again? (Y)");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        await Execute();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ExchangeRateService encountered an unhandled exception: {ex.Message}");
            }
        }
    }
}
