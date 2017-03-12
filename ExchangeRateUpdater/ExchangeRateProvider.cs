using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    using System;
    using System.Globalization;

    public class ExchangeRateProvider
    {
        const string query = "en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";

        private readonly HttpClient client;

        public ExchangeRateProvider()
        {
            this.client = new HttpClient("https://www.cnb.cz/");
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var response = this.client.Get(query);

            return
                this.ParseToExchangeRates(response)
                    .Where(x => currencies.Any(currency => currency.Code == x.SourceCurrency.Code));
        }

        private IEnumerable<ExchangeRate> ParseToExchangeRates(string response)
        {
            return
                response.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(2)
                    .Select(this.ParseExchangeRate);
        }

        private ExchangeRate ParseExchangeRate(string text)
        {
            var line = text.Split('|');
            var rate = Convert.ToDecimal(line[4], CultureInfo.InvariantCulture);
            var amount = Convert.ToInt32(line[2]);
            var code = line[3];

            return new ExchangeRate(new Currency(code), new Currency("CZK"), rate / amount);
        }
    }
}