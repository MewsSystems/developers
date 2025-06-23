using ExchangeRateUpdater.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider(IExchangeRateApiClientFactory exchangeRateApiClientFactory) : IExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, string targetCurrencyCode)
        {
            if (!currencies.Any())
            {
                return [];
            }

            var exchangeRateApiClient = exchangeRateApiClientFactory.CreateExchangeRateApiClient(targetCurrencyCode);

            var apiExchangeRates = await exchangeRateApiClient.GetDailyExchangeRatesAsync();
            var apiExchangeRatesByCode = apiExchangeRates.ToDictionary(x => x.CurrencyCode);

            var targetCurreny = exchangeRateApiClient.TargetCurrency;

            return currencies
                .DistinctBy(c => c.Code)
                .Select(c => 
                {
                    var apiExchangeRate = apiExchangeRatesByCode.GetValueOrDefault(c.Code);
                    var value = apiExchangeRate == null 
                        ? default 
                        : apiExchangeRate.Rate / apiExchangeRate.Amount;

                    return new ExchangeRate(c, targetCurreny, value);
                })
                .Where(x => x.Value != default)
                .ToArray();
        }
    }
}
