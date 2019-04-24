using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ExchangeRateRestClient _restClient = new ExchangeRateRestClient();
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRate>();
            var response = _restClient.GetExchangeRate();
            var codes = currencies.Select(a => a.Code).ToList();
            var dictionary = currencies.ToDictionary(a => a.Code);
            var exchangeRows = response.Table.Rows.Where(a => codes.Contains(a.Code)).ToList();

            foreach (var row1 in exchangeRows)
            {
                foreach(var row2 in exchangeRows)
                {
                    var source = dictionary[row1.Code];
                    var target = dictionary[row2.Code];
                    var exrate = (Decimal.Parse(row1.Rate) * row2.Amount) / (Decimal.Parse(row2.Rate) * row1.Amount);
                    exchangeRates.Add(new ExchangeRate(source, target, exrate));
                }
            }

            return exchangeRates;
        }
    }
}
