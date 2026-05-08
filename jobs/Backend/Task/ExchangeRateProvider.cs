using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.HttpClients;
using ExchangeRateUpdater.Parsers;

namespace ExchangeRateUpdater
{
    // Provider for fetching exchange rates from CNB.
    public class ExchangeRateProvider(ICnbHttpClient httpClient, ICnbDataParser parser)
    {
        private readonly ICnbHttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        private readonly ICnbDataParser _parser = parser ?? throw new ArgumentNullException(nameof(parser));

        // Fetches exchange rates for the specified currencies parameter from CNB.
        // Returns only rates that are explicitly defined by the source - no calculated or inverse rates.
        // If a currency is not available from CNB, it is silently ignored.
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            // Fetch raw data from CNB API
            var rawData = await _httpClient.GetExchangeRatesAsync();

            // Parse the received data
            var parsedData = _parser.Parse(rawData);

            // Filter to only requested currencies (case-insensitive comparison)
            var requestedCodes = new HashSet<string>(
                currencies.Select(c => c.Code),
                StringComparer.OrdinalIgnoreCase);

            var filteredData = parsedData.Where(d => requestedCodes.Contains(d.Code));

            // Map to ExchangeRate domain models
            // CNB provides: Amount units of foreign currency = Rate CZK
            // We want: 1 CZK = X units of foreign currency
            // Formula: X = Amount / Rate
            var czk = new Currency("CZK");

            return filteredData.Select(d => new ExchangeRate(
                sourceCurrency: czk,
                targetCurrency: new Currency(d.Code),
                value: d.Amount / d.Rate
            )).ToList();
        }
    }
}
