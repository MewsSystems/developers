using ExchangeRateUpdater.API.CNB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ICNBApiClient _cnbApiClient;
        public ExchangeRateProvider(ICNBApiClient cnbApiClient)
        {
            _cnbApiClient = cnbApiClient;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// 

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currencyCodes = currencies.Select(c => c.Code).ToList();

            var result = await _cnbApiClient.GetDailyRates();

            return result.Rates?.Where(r => currencyCodes.Contains(r.CurrencyCode))
                .Select(r => new ExchangeRate(new Currency(r.CurrencyCode), new Currency("CZK"), r.Rate / r.Amount));
        }
    }
}
