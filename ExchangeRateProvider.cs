using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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
            try
            {
                var unformattedRates = GetRatesFile();
                if (unformattedRates.Count() > 0)
                {
                    return PopulateExchangeRates(unformattedRates, currencies);
                }

                return Enumerable.Empty<ExchangeRate>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
                throw;
            }
        }

        /// <summary>
        /// Determines the exchange rates that should be displayed based on the input list of currencies
        /// </summary>
        private List<ExchangeRate> PopulateExchangeRates(List<string> rawRates, IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();

            try
            {
                foreach(var currency in currencies)
                {
                    if(currency.Code == "CZK")
                    {
                        rates.Add(new ExchangeRate(currency, new Currency("CZK"), 1.00m));
                    } else
                    {
                        var strRate = rawRates.Where(x => x.Contains(currency.Code)).FirstOrDefault();
                        if(strRate != null)
                        {
                            rates.Add(new ExchangeRate(currency, 
                                new Currency("CZK"), 
                                decimal.Parse(strRate.Substring(strRate.LastIndexOf('|') + 1))));
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to format exchange rates; Error message: '{e.Message}'.");
                throw;
            }
            return rates;
        }

        /// <summary>
        /// Retrieves the currency exchange rates from the CNB website
        /// </summary>
        private List<string> GetRatesFile()
        {
            List<string> exchangeRates = new List<string>();
            try
            {
                WebClient client = new WebClient();
                string url = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";
                byte[] data = client.DownloadData(url);
                string unformattedData = System.Text.Encoding.UTF8.GetString(data);
                
                return unformattedData.Split(Environment.NewLine.ToCharArray()).Skip(2).ToList();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"Unable to retrieve rates; Error message: '{e.Message}'.");
                throw;
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine($"Error populating rates; Error message: '{e.Message}'.");
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred retrieving exchange rates; Error message: '{e.Message}'.");
                throw;
            }
        }
    }
}
