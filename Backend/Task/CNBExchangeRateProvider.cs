using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Helpers;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRateProvider
    {
        private const string CnbExhangeRatesEndpoint = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return GetExchangeRatesAsync(currencies).Result;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var response = await HttpClientHelper.GetResponseFromUrl(CnbExhangeRatesEndpoint);
            var exchangeRatesTicket = CNBExchangeRateParser.ParseContent(response);
            var filteredTicketItems = exchangeRatesTicket.Items.Where(x =>
                currencies.Any(y => string.Equals(y.Code, x.Code, StringComparison.InvariantCultureIgnoreCase)));
            var exchangeRates = filteredTicketItems.Select(x => new ExchangeRate(
                sourceCurrency: new Currency(x.Code),
                targetCurrency: new Currency("CZK"),
                value: x.Rate
            ));

            return exchangeRates;
        }
    }
}
