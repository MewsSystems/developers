using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ICzechNationalBankExchangeRateGateway _rateGateway;

        public ExchangeRateProvider(ICzechNationalBankExchangeRateGateway rateGateway)
        {
            _rateGateway = rateGateway;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRates = _rateGateway.GetCurrentRates();
            var result = new List<ExchangeRate>();
            var sourceCurrency = new Currency("CZK");
            foreach (var rate in exchangeRates.Rates)
            {
                if (currencies.Any(x => x.Code == rate.CurrencyCode))
                {
                    var targetCurrency = new Currency(rate.CurrencyCode);
                    var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, rate.Rate);
                    result.Add(exchangeRate);
                }
            }

            return result;
        }
    }
}