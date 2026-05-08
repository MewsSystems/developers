using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace ExchangeRateUpdater.Services
{
    /// <summary>
    /// Generic exchange rate provider that orchestrates data fetching and parsing.
    /// It is reusable across different exchange rate sources by injecting different implementations
    /// of IExchangeRateApiClient and IExchangeRateDataParser.
    /// </summary>
    public class ExchangeRateProvider
    {
        private readonly IExchangeRateApiClient _apiClient;
        private readonly IExchangeRateDataParser _dataParser;

        public ExchangeRateProvider(IExchangeRateApiClient apiClient, IExchangeRateDataParser dataParser)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _dataParser = dataParser ?? throw new ArgumentNullException(nameof(dataParser));
        }

        /// <summary>
        /// Gets exchange rates for specified currencies.
        /// Returns only rates explicitly provided by the source (no calculated/reverse rates).
        /// Cancellation token allows cooperative cancellation of async operations
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
            IEnumerable<Currency> currencies,
            CancellationToken cancellationToken = default)
        {
            var currenciesList = currencies?.ToList() ?? throw new ArgumentNullException(nameof(currencies));

            if (!currenciesList.Any())
                return Enumerable.Empty<ExchangeRate>();

            var rawData = await _apiClient.GetDailyRatesAsync(cancellationToken);
            return _dataParser.Parse(rawData, currenciesList);
        }
    }
}