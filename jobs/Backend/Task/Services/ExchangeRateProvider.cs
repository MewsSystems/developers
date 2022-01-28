using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateSource exchangeRateSource;

        public ExchangeRateProvider(IExchangeRateSource exchangeRateSource)
        {
            this.exchangeRateSource = exchangeRateSource;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var enumerableExchangeRates = exchangeRateSource.GetAllExchangeRates();
            var requestedExchangedRates = currencies.ToLookup(x => $"{x.Code}/CZK");

            return enumerableExchangeRates.Where(x => requestedExchangedRates.Contains($"{x.SourceCurrency}/{x.TargetCurrency}"));
        }

    }
}
