using System.Collections.Generic;
using System.Linq;
using ExchangeRateProvider.Models;
using HtmlAgilityPack;

namespace ExchangeRateProvider.Services
{
    public class ExchangeRateFixingHtmlParser : IExchangeRatesParser
    {
        public IEnumerable<ExchangeRate> ExtractCurrencyExchangeRates(string targetCurrencyCode, string source)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(source);

            const string currencyTableXpath = "//table[@class='currency-table']//tbody//tr";
            const string currencyColumnsXpath = "td";

            var nodes = doc.DocumentNode.SelectNodes(currencyTableXpath);
            foreach (var node in nodes)
            {
                var childNodes = node.SelectNodes(currencyColumnsXpath).Select(n => n.InnerText).ToList();
                var currencyCode = childNodes[3];
                var amount = int.Parse(childNodes[2]);
                var rate = decimal.Parse(childNodes[4]);
                yield return new ExchangeRate(new Currency(currencyCode), new Currency(targetCurrencyCode), rate / amount);
            }
        }
    }
}
