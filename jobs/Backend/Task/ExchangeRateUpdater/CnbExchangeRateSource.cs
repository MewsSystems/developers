using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateSource
    {
        private const string TargetCurrency = "CZK";

        private readonly string _url;

        public CnbExchangeRateSource(Options options)
        {
            _url = options.Url ?? throw new ArgumentNullException(nameof(Options.Url));
        }

        public async Task<ExchangeRates> GetLatestExchangeRatesAsync()
        {
            var content = await _url.GetStringAsync();
            return ParseRates(content);
        }

        /// <summary>
        /// Parses exchange rate according to the specification at https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates/
        /// </summary>
        private ExchangeRates ParseRates(string content)
        {
            var lines = content.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var dateLine = lines.FirstOrDefault() ?? throw new Exception("No exchange rate information was found.");

            var dateString = dateLine.Split("#").First().Trim();
            var date = DateOnly.Parse(dateString, CultureInfo.InvariantCulture);

            var rates = lines.Skip(2).Select(ParseRate).ToArray();

            return new ExchangeRates
            {
                Date = date,
                Rates = rates
            };
        }

        private ExchangeRate ParseRate(string s)
        {
            var parts = s.Split("|");

            if (parts.Count() < 5)
            {
                throw new Exception("Exchange rate format error.");
            }

            return new ExchangeRate(new Currency(parts[3]), new Currency(TargetCurrency), decimal.Parse(parts[4], CultureInfo.InvariantCulture) / int.Parse(parts[2]));
        }

        public record Options
        {
            public string Url { get; set; }
        }
    }
}
