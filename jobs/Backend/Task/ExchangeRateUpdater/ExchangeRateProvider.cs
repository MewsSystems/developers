using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider(ICzechNationalBankApiClient czechNationalBankApiClient)
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (!currencies.Any())
            {
                return [];
            }

            var currentExchangeRates = await czechNationalBankApiClient.GetDailyExchangeRatesAsync();

            var currentExchangeRatesByCode = currentExchangeRates.ToDictionary(x => x.CurrencyCode);

            var targetCurreny = new Currency(czechNationalBankApiClient.TargetCurrencyCode);

            return currencies
                .Select(c => new ExchangeRate(c, targetCurreny, currentExchangeRatesByCode.GetValueOrDefault(c.Code)?.Rate ?? default))
                .Where(x => x.Value != default)
                .ToArray();
        }
    }
}
