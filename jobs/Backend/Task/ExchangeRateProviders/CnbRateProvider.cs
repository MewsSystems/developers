using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateProviders
{
    /// <summary>
    /// Czech National Bank provider 
    /// <see cref="https://www.cnb.cz/en/"/>
    /// </summary>
    public class CnbRateProvider : ExchangeRateProvider
    {

        /*
            01 Oct 2021 #190
            Country|Currency|Amount|Code|Rate
            Australia|dollar|1|AUD|15.834
            Brazil|real|1|BRL|4.030
            ...
         * 
         */

        private static string ProviderUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private static RegionInfo CzechRepublicRegion = new RegionInfo("cs-CZ");

        private static Currency BaseCurrency = new Currency(CzechRepublicRegion.ISOCurrencySymbol);

        private async Task<string> GetQuotesTxt(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync(url);
                return result;
            }
        }

        private IDictionary<Currency, decimal> ParseInfo(string quotes)
        {
            int dateLen = 11;
            string headers = "Country|Currency|Amount|Code|Rate";

            var quotesLines = quotes.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            //Checking txt header format
            if (DateTime.TryParse(quotesLines[0].Substring(0,11), out var quotesDate) && quotesLines[1] == headers)
            {
                return quotesLines
                    .Skip(2)
                    .Select(line =>
                    {
                        var columns = line.Split('|');
                        var amount = int.Parse(columns[2]);
                        var currency = new Currency(columns[3]);
                        var rate = decimal.Parse(columns[4]);

                        return (currency, rate / amount);
                    })
                    .ToDictionary(pair => pair.currency, pair => pair.Item2);
            }
            else
            {
                throw new Exception("Could not parse quote header. Check for structure changes");
            }
        }

        public override IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var task = Task.Run(async () => await GetQuotesTxt(CnbRateProvider.ProviderUrl));
            var parsedRates = ParseInfo(task.GetAwaiter().GetResult());

            return currencies
                .Where(curr => parsedRates.ContainsKey(curr))
                .Select(quote => new ExchangeRate(quote, CnbRateProvider.BaseCurrency, parsedRates[quote]))
                .ToList();
        }
    }
}
