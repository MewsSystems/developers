using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Cnb;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ICnbClient _cnbClient;
        private readonly CnbMapper _cnbMapper;

        public ExchangeRateProvider(ICnbClient cnbClient)
        {
            _cnbClient = cnbClient;
            _cnbMapper = new CnbMapper();
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var dailyRates = await _cnbClient.GetLatestExchangeRatesAsync();
            var mapped = dailyRates.Rates
                .Select(_cnbMapper.MapExchangeRate)
                .ToArray();
                return mapped.Where(r => currencies.Any(c => c.Code == r.SourceCurrency.Code));
        }
    }
}
