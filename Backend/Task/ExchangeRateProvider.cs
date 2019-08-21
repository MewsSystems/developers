using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string ExchangeRatesUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market" +
            "/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        private readonly Currency CzkCurrency = new Currency("CZK");

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || !currencies.Any())
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            var result = new List<ExchangeRate>();

            var response = GetExchangeData();

            var extractedExchangeData = GetExtractedExchangeData(response);

            foreach (var item in extractedExchangeData)
            {
                var lineArray = item.Split('|');

                if (lineArray.Length < 3) continue;

                var currency = lineArray[1];

                if (!currencies.Any(x => x.Code == currency)) continue;

                decimal.TryParse(lineArray[0], out decimal source);
                decimal.TryParse(lineArray[2], out decimal target);

                result.Add(new ExchangeRate(
                    new Currency(lineArray[1]),
                    CzkCurrency,
                    target / source));
            }

            return result;
        }

        private string GetExchangeData()
        {
            using (var client = new HttpClient())
            {
                var responseResult = client.GetAsync(ExchangeRatesUrl).Result;

                if (responseResult.IsSuccessStatusCode)
                {
                    return responseResult.Content.ReadAsStringAsync().Result;
                }
            }

            return string.Empty;
        }

        private IEnumerable<string> GetExtractedExchangeData(string response)
        {
            Regex r = new Regex(@"(\d+)(\d|.+)");

            var regexMatches = r.Matches(response).Cast<Match>()
                .Select(x => x.Value);

            return regexMatches.Skip(1);
        }
    }
}
