using ExchangeRateUpdaterV2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterV2.Contracts
{
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Provider name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        IAsyncEnumerable<ExchangeRate> GetExchangeRates();
    }
}
