using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Fetch;
using ExchangeRateUpdater.Parse;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRatesTxtFetcher exchangeRatesTxtFetcher;
        private readonly IExchangeRatesParser exchangeRatesParser;

        public ExchangeRateProvider(IExchangeRatesTxtFetcher exchangeRatesTxtFetcher, IExchangeRatesParser exchangeRatesParser)
        {
            this.exchangeRatesTxtFetcher = exchangeRatesTxtFetcher ?? throw new System.ArgumentNullException(nameof(exchangeRatesTxtFetcher));
            this.exchangeRatesParser = exchangeRatesParser ?? throw new System.ArgumentNullException(nameof(exchangeRatesParser));
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var ratesTxt = exchangeRatesTxtFetcher.FetchExchangeRates().GetAwaiter().GetResult();
            var currenciesCodesSet = currencies.Select(x => x.Code).ToHashSet();
            var parsedRates = exchangeRatesParser.ParseRates(ratesTxt);

            return parsedRates
                .Where(rate => currenciesCodesSet.Contains(rate.SourceCurrency.Code)
                    || currenciesCodesSet.Contains(rate.TargetCurrency.Code));
        }
    }
}
