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

        public async Task<List<Currency>> GetExchangeRates()
        {
            return await GetDataFromSource();
        }

        private async Task<List<Currency>> GetDataFromSource()
        {
            using (var client = new HttpClient())
            {
                var html = await client.GetStringAsync(URL);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var currencyData = doc.DocumentNode.SelectSingleNode("//table[@class='currency-table']");
                List<List<string>> listOfCurrencies = currencyData.Descendants("tr").Select(tr => tr.Descendants("td")
                                        .Select(td => WebUtility.HtmlDecode(td.InnerText)).ToList()).ToList();

                List<Currency> currencyList = new List<Currency>();

                foreach (var item in listOfCurrencies)
                {
                    if (string.IsNullOrEmpty(item.FirstOrDefault()))
                    {
                        continue;
                    }

                    else
                    {
                        Currency currency = new Currency();

                        foreach (var value in item)
                        {
                            currency.Country = item[0];
                            currency.CurrencyName = item[1];
                            currency.Amount = item[2];
                            currency.Code = item[3];
                            currency.Rate = item[4];
                        }

                        currencyList.Add(currency);
                    }
                }
                return currencyList;
            }
        }
    }
}
