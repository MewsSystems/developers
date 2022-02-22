using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.DataSource;
using ExchangeRateUpdater.Deserializers;
using ExchangeRateUpdater.DTO;

namespace ExchangeRateUpdater.Provider
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateDataSourceProvider _dataSourceProvider;
        private readonly IExchangeRatesDeserializer _deserializer;

        public ExchangeRateProvider(IExchangeRateDataSourceProvider dataSourceProvider, IExchangeRatesDeserializer deserializer)
        {
            _dataSourceProvider = dataSourceProvider;
            _deserializer = deserializer;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var serialized = _dataSourceProvider.Get();
            var rates = _deserializer.Deserialize(serialized).Where(r => r != null);
            return rates.Join(currencies, r => r.SourceCurrency.Code, c => c.Code, (r, _) => r);
        }
    }
}
