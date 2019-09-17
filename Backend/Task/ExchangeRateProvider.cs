using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string CNBUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";

        private const string TargetCurrencyName = "CZK";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var content = await GetExchangeRatesFromNBC();

            var targetCurrency = currencies.FirstOrDefault(cur => cur.Code == TargetCurrencyName);

            var matches = Regex
                .Matches(content, "(?<=\\n)(.*?)(?=\\n)")
                .Cast<Match>()
                .Skip(1)
                .Select(match =>
                {
                    var values = match.Value.Split('|');

                    if (decimal.TryParse(values[2], out decimal amount) &&
                        decimal.TryParse(values[4], out decimal rate) &&
                        currencies.FirstOrDefault(cur => cur.Code == values[3]) is Currency sourceCurrency)
                    {
                        return new ExchangeRate(sourceCurrency, targetCurrency, rate, amount);
                    }

                    return null;
                })
                .Where(er => er is ExchangeRate);

            return matches;
        }

        private async Task<string> GetExchangeRatesFromNBC()
        {
            byte[] content;
            using (var wc = new WebClient())
            {
                content = await wc.DownloadDataTaskAsync(new Uri(CNBUrl));
            }

            return Encoding.UTF8.GetString(content);
        }
    }
}
