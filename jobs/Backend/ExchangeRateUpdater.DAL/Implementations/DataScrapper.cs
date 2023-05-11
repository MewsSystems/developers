using ExchangeRateUpdater.DAL.Interfaces;
using ExchangeRateUpdater.DAL.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ExchangeRateUpdater.DAL.Implementations
{
    public class DataScrapper : IDataScrapper
    {
        private readonly ILogger<DataScrapper> _logger;
        public DataScrapper(ILogger<DataScrapper> logger) 
        {
            _logger = logger;
        }
        public List<RateModel> GetRatesFromWeb(string URL)
        {
            List<RateModel> exchangeRatesTable = new List<RateModel>();
            // Download the webpage content using a web client
            WebClient client = new WebClient();

            try
            {                                
                string htmlContent = client.DownloadString(URL);

                // Parse the HTML content using HtmlAgilityPack
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // Find the table element using its class attribute
                HtmlNode tableNode = doc.DocumentNode.SelectSingleNode("//table[@class='currency-table']");

                // Iterate through the rows and columns of the table and add them to the list object
                if (tableNode != null)
                {
                    HtmlNode tablebodyNode = doc.DocumentNode.SelectSingleNode("//tbody");
                    foreach (HtmlNode row in tablebodyNode.SelectNodes("tr"))
                    {
                        var x = row.SelectNodes("td");
                        RateModel rm = new RateModel();
                        rm.Country = x[0].InnerText;
                        rm.Currency = x[1].InnerText;
                        rm.Amount = int.Parse(x[2].InnerText);
                        rm.Code = x[3].InnerText;
                        rm.Rate = decimal.Parse(x[4].InnerText);
                        exchangeRatesTable.Add(rm);
                    }
                }
            }
            catch (WebException ex)
            {
                _logger.LogError($"Web Exception Error found in GetRatesFromWeb Method. Message: {ex.Message}");
                throw ex;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Exception Error found in GetRatesFromWeb Method. Message: {ex.Message}");
            }
            finally {
                client.Dispose();
            }
            return exchangeRatesTable;
        }
    }
}
