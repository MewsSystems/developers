using ExchangeRateUpdater.CNB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace ExchangeRateUpdater
{
    


    public class ExchangeRateProvider
    {
        public const string CZK = "CZK";

        private readonly ICnbClient client;

        public ExchangeRateProvider(ICnbClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return client
                .GetExchangeRates()
                .Table
                .Select(record => new ExchangeRate(
                    new Currency(CZK),
                    new Currency(record.Code),
                    record.Rate / record.Quantity))
                .Where(exchangeRate => currencies.Any(currency =>
                       currency.Code == exchangeRate.SourceCurrency.Code
                    || currency.Code == exchangeRate.TargetCurrency.Code));
        }
    }
}
