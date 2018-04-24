using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if(currencies == null || currencies.Count() == 0)
            {
                Console.WriteLine("No currency is provided!");
                return Enumerable.Empty<ExchangeRate>();
            }

            List<ExchangeRate> addedExRates = new List<ExchangeRate>();

            try
            {
                //Since the Czech National Bank offical website has no any RSS feed for exchange rate, we parse the daily exchange 
                //rate page to get list of currencies with their daily ex. rate 

                string url = @"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.jsp";
                string xPath = @"//*[@id='main-box']/table";
                string sourceCurrency = "CZK";

                var rateDtoList  = PullDataFromWebPage(url, xPath);
                if(rateDtoList.Count > 0)
                {
                    string[] codes = currencies.Select(c => c.Code).ToArray();
                    var filtered = rateDtoList.Where(r => codes.Contains(r.Name.Trim())).ToList();
                    if(filtered != null && filtered.Count > 0)
                    {
                        addedExRates.AddRange(filtered.Select(f => new ExchangeRate(new Currency(sourceCurrency), new Currency(f.Name), f.Rate)).ToList());
                    }

                }else
                {
                    Console.WriteLine("No exchange rate is fetched from the source!");
                }

                return addedExRates.AsEnumerable();

            }catch(Exception ex)
            {
                //Save some log information about the exception and return any added exchange rates
                Console.WriteLine(String.Format("Error in getting exchange rates. Detail:: {0}", ex.InnerException == null ? ex.Message : ex.InnerException.Message));

                return addedExRates;
            }
        }

        /// <summary>
        /// Returns list of RateDTO (inner class) after parsing the web page of the provided url and nodes under the provided xPath
        /// This method uses the package called HtmlAgilityPack (from nuget) library which helps us to load web page and parses through the nodes
        /// </summary>
        /// <param name="url"> the url of the web page </param>
        /// <param name="xPath">the xPath of the specific node to parse from</param>
        /// <returns></returns>
        private List<RateDTO> PullDataFromWebPage(string url, string xPath)
        {
            List<RateDTO> rateDTOList = new List<RateDTO>();
            

            Console.WriteLine("Getting Exchange Rates. Please Wait....");

            var web = new HtmlWeb();
            var doc = web.Load(url);

            
            var value = doc.DocumentNode.SelectSingleNode(xPath);
            if(value != null)
            {
                var tableRows = value.Descendants("tr");
                // the first row is header(title) row, so we need data starting from row with index = 1
                if (tableRows != null && tableRows.Count() > 1)
                {
                    for(int i = 1; i < tableRows.Count(); i++)
                    {
                        var tableColumns = tableRows.ElementAt(i);

                        //From the Czech National Bank website, we know that the table has five columns and the 4th and 5th one holds the code and rate values respectively.
                        if (tableColumns != null && tableColumns.ChildNodes != null && tableColumns.ChildNodes.Count == 5)
                        {
                            var name = tableColumns.ChildNodes.ElementAt(3).InnerText;
                            var rate = tableColumns.ChildNodes.ElementAt(4).InnerText;

                            RateDTO dto = new RateDTO()
                            {
                                Name = name,
                                Rate = Convert.ToDecimal(rate)
                            };
                            rateDTOList.Add(dto);
                            
                        }
                    }
                }
            }

            

            return rateDTOList;
        }


        //Inner class to hold the parsed Name/Rate Pair
        private class RateDTO
        {
            public string Name { get; set; }
            public decimal Rate { get; set; }
        }
    }
}
