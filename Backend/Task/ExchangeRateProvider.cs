using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateRepository repository;

        // Design decisions: Since no IoC/Factory infrastructure implemented CNBExchangeRepository is injected as default implementation of IExchangeRateRepository. 
        public ExchangeRateProvider() : this(new CNBExchangeRepository(new CNBExchangeRateParsingStrategy(), new HttpClientFactory()))
        {
        }

        public ExchangeRateProvider(IExchangeRateRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            bool specifiedExchangeRate(ExchangeRate rate) =>
                currencies.Any(currency => currency.Code == rate.SourceCurrency.Code)
                && currencies.Any(currency => currency.Code == rate.TargetCurrency.Code);

            return this.repository.GetExchangeRates().Where(specifiedExchangeRate);
        }
    }
}
