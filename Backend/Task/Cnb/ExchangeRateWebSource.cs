using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Cnb
{
    public sealed class ExchangeRateWebSource : IExchangeRateSource
    {
        private readonly Currency targetCurrency;
        private readonly string[] sourceUrls;
        private readonly ExchangeRateParser parser;

        public ExchangeRateWebSource()
            : this(new Currency("CZK"), 
                "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt",
                "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt")
        {
        }

        public ExchangeRateWebSource(Currency sourceCurrency, params string[] sourceUrls)
        {
            if (sourceUrls == null || sourceUrls.Length == 0)
                throw new ArgumentException($"No source url provided. {nameof(ExchangeRateWebSource)} requires at least one url.", nameof(sourceUrls));
            this.targetCurrency = sourceCurrency;
            this.sourceUrls = sourceUrls;
            parser = new ExchangeRateParser();
        }

        public async Task<ExchangeRate[]> Load()
        {
            var tasks = sourceUrls.Select(url => Task.Run(() => GetData(url)));
            var result = await Task.WhenAll(tasks);

            return result.SelectMany(x => x).ToArray();
        }

        private async Task<IEnumerable<ExchangeRate>> GetData(string url)
        {
            using (var client = new HttpClient())
            {
                var rawData = await client.GetStringAsync(url);
                return parser.Parse(rawData, targetCurrency);
            }
        }
    }
}
