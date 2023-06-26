using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater;

public class HtmlParserService : IHtmlParserService
{
    private readonly string URL = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/";

    public async Task<List<List<string>>> GetDataFromSource()
    {
        using var client = new HttpClient();
        var html = await client.GetStringAsync(URL);
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);

        var currencyData = doc.DocumentNode.SelectSingleNode("//tbody");
        var listOfCurrencies = currencyData.Descendants("tr")
            .Select(tr => tr.Descendants("td")
                .Select(td => WebUtility.HtmlDecode(td.InnerText)).ToList())
            .ToList();


        return listOfCurrencies;
    }
}