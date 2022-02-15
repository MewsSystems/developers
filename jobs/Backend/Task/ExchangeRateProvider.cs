using System;
using System.Collections.Generic;
using ExchangeRateUpdater.Fluent;
using ExchangeRateUpdater.Html;
using ExchangeRateUpdater.Http;
using ExchangeRateUpdater.Xml;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async IAsyncEnumerable<ExchangeRate> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            using var fluentState = new FluentState();

            var exchangeRates = fluentState
                .ReadStreamFromHttp(new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/" +
                                    "central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/"))
                .ReadElementAsString(new ElementDescriptor("table").WithClass("currency-table"))
                .ToXmlElement()
                .CreateTable(Constants.ExchangeRateColumnNames)
                .ComputeExchangeRates(currencies);

            await foreach (var exchangeRate in exchangeRates)
            {
                yield return exchangeRate;
            }
        }
    }
}
