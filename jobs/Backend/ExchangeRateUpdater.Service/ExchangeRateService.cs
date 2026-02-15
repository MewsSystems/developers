using ExchangeRateUpdater.Abstraction.Interfaces;
using ExchangeRateUpdater.Abstraction.Model;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Service
{
    /// <summary>
    /// Provides functionality to retrieve exchange rates for specified currency codes.
    /// </summary>
    /// <param name="exchangeRateProvider">The provider used to fetch exchange rate data.</param>
    /// <param name="logger">The logger instance used to record informational and error messages for this service.</param>
    public class ExchangeRateService(IExchangeRateProvider exchangeRateProvider, ILogger<ExchangeRateService> logger) : IExchangeRateService
    {
        /// <summary>
        /// Retrieves exchange rates for the specified collection of currency codes.
        /// </summary>
        /// <param name="currencyCodes">An enumerable collection of currency code strings to retrieve exchange rates for. Each string must represent
        /// a valid currency code.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of exchange rates
        /// corresponding to the valid currency codes provided. Returns an empty collection if no valid codes are
        /// supplied.</returns>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesFromStringList(IEnumerable<string> currencyCodes)
        {
            List<Currency> currencyList = new();
            foreach (var currencyCode in currencyCodes)
            {
                try
                {
                    var currency = new Currency(currencyCode);
                    currencyList.Add(currency);
                }
                catch (ArgumentException ex)
                {
                    logger.LogWarning($"Invalid currency {currencyCode}: {ex.Message}");
                    continue;
                }
            }

            if (currencyList.Count == 0)
            {
                logger.LogWarning("No valid currency codes provided.");
                return [];
            }

            IEnumerable<ExchangeRate> exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(currencyList);
            return exchangeRates;
        }
    }
}
