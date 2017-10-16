using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var targetCurrency = new Currency("CZK");

            try
            {
                var rates = CreateExchangeRates(targetCurrency);

                return rates.Where(t => currencies.Any(c => c.Code == t.SourceCurrency.Code) && currencies.Any(c => c.Code == t.TargetCurrency.Code));
            }
            catch (Exception ex)
            {
                throw new Exception($"Getting rates from National Bank failed. Addiotional information: '{ex.Message}'");
            }
        }

        private IEnumerable<ExchangeRate> CreateExchangeRates(Currency targetCurrency)
        {
            return NationalBankRateProvider.GetBankRates().Select(t => new ExchangeRate(new Currency(t.Code.ToUpper()), targetCurrency, t.Rate));
        }
    }
}

