using ExchangeRateUpdater.ExchangeRateDataProviders;
using ExchangeRateUpdater.ExchangeRateParser;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRateDataSource dataProvider;
        private readonly IExchangeRateParser parser;

        public ExchangeRateProvider(IExchangeRateDataSource dataProvider, IExchangeRateParser parser)
        {
            this.dataProvider = dataProvider;
            this.parser = parser;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CancellationToken ct = default)
        {
            var data = await dataProvider.GetDataAsync(ct);
            var rates = parser.Parce(data);

            return rates.Where(r => currencies.Any(c => c.Code == r.TargetCurrency.Code));
        }
    }
}
