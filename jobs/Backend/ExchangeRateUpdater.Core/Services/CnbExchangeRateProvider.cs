using ExchangeRateUpdater.Core.Clients;
using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Core.Services
{
    /// <summary>
    /// Processes exchange rates from the Czech National Bank (CNB) API.
    /// </summary>
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<CnbExchangeRateProvider> _logger;
        private readonly ICnbExchangeRateClient _client;

        public CnbExchangeRateProvider(ILogger<CnbExchangeRateProvider> logger, ICnbExchangeRateClient client)
        {
            _logger = logger;
            _client = client;
        }

        /// <summary>
        /// Fetches and processes exchange rates from the CNB API.
        /// </summary>
        /// <param name="currencies">List of currencies to filter.</param>
        /// <param name="date">The date for which exchange rates are requested.</param>
        /// <returns>An asynchronous stream of processed exchange rate data.</returns>
        public async IAsyncEnumerable<ExchangeRate> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime date)
        {
            var dateString = date.ToString("yyyy-MM-dd");

            // Fetch raw exchange rate data from CNB API
            var cnbResponse = await _client.GetRatesAsync(dateString);
            if (cnbResponse is null)
            {
                _logger.LogWarning("CNB response is null for date {Date}.", dateString);
                yield break; // Exit early if there's no data
            }

            var sourceCurrency = new Currency("CZK"); // CNB always provides rates relative to CZK
            var requestedCodes = currencies.Select(c => c.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var item in cnbResponse.Rates)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.CurrencyCode))
                    continue;

                if (requestedCodes.Contains(item.CurrencyCode))
                {
                    // Normalize exchange rate based on the amount (if applicable)
                    var rateValue = item.Rate;
                    if (item.Amount > 0 && rateValue > 0)
                    {
                        rateValue /= item.Amount;
                    }

                    _logger.LogDebug("Processed rate for {CurrencyCode}: {RateValue} (Amount: {Amount}, Country: {Country}, Order: {Order})",
                        item.CurrencyCode, rateValue, item.Amount, item.Country, item.Order);

                    // Yield the mapped domain entity
                    yield return new ExchangeRate(
                        sourceCurrency,
                        new Currency(item.CurrencyCode),
                        rateValue,
                        item.ValidFor
                    );
                }
            }
        }
    }
}
