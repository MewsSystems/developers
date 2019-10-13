using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System;
using System.Globalization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private static readonly Currency CZECH_CROWN = new Currency("CZK");

        private static readonly HttpClient client = new HttpClient();

        private static ExchangeRate ParseLine(string line)
        {
            try
            {
                string[] values = line.Split('|');
                decimal amount = Decimal.Parse(values[2]);
                string code = values[3];
                decimal value = Decimal.Parse(
                    values[4]
                    .Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator));
                return new ExchangeRate(new Currency(code), CZECH_CROWN, value / amount);
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to parse line '" + line + "'", ex);
            }
        }

        private static Dictionary<string, ExchangeRate> ParseResponse(string response)
        {
            Dictionary<string, ExchangeRate> rates = new Dictionary<string, ExchangeRate>();
            string [] lines = response.Split('\n');
            if (lines.Length > 2)
            {
                for(int i  = 2; i < lines.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        var rate = ParseLine(lines[i]);
                        rates.Add(rate.SourceCurrency.Code, rate);
                    }
                }
            }
            return rates;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
                return Enumerable.Empty<ExchangeRate>();

            string response = client.GetStringAsync("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt").Result;
            var result = ParseResponse(response);
            List<ExchangeRate> rates = new List<ExchangeRate>();

            foreach(var c in currencies) 
            {
                if (result.ContainsKey(c.Code))
                {
                    rates.Add(result[c.Code]);
                }
            }

            return rates;
        }
    }
}
