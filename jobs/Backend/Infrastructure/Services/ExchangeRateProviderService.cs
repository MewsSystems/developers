using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Entities;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<Row> GetExchangeRatesAsync(IEnumerable<Currency> currencies, ExchangeRate rateModel)
        {
            return  rateModel.Table.Rows.Where(b => currencies.Any(a => b.Code.ToLower().Equals(a.Code.ToLower()))).ToList();
        }

    }
}
