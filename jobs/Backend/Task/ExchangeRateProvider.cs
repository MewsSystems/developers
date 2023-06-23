using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly string URL = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<List<List<string>>> GetExchangeRates()
        {
            List<List<string>> exchangeRates = new List<List<string>>();
            exchangeRates = await GetDataFromScource();
            return exchangeRates;
        }

        private async Task<List<List<string>>> GetDataFromScource()
        {

            using (var client = new HttpClient())
            {
                var html = await client.GetStringAsync(URL);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var table = doc.DocumentNode.SelectSingleNode("//table[@class='currency-table']");
                List<List<string>> list = table.Descendants("tr").Select(tr => tr.Descendants("td")
                                        .Select(td => WebUtility.HtmlDecode(td.InnerText)).ToList()).ToList();

                int cap = list.Count();

                return list;
            }
        }
    }
}
