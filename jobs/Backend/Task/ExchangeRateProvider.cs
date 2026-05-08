using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.ExchangeClients;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRateClient _client;
        private readonly IExchangeRateDataParser _parser;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(IExchangeRateClient client, IExchangeRateDataParser parser, ILogger<ExchangeRateProvider> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
        {
            var currenciesList = currencies?.ToList() ?? throw new ArgumentNullException(nameof(currencies));

            if (!currenciesList.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            var rawData = await _client.GetDailyRatesAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(rawData))
            {
                _logger.LogError("Received empty data from exchange client.");
                throw new ExchangeRateUpdateException("Received empty data from exchange client.");
            }

            IEnumerable<ExchangeRate> parsedData;
            try
            {
                parsedData = _parser.Parse(rawData, currenciesList).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse exchange rates.");
                throw new ExchangeRateUpdateException("Failed to parse exchange rates.", ex);
            }

            return parsedData;
        }
    }
}
