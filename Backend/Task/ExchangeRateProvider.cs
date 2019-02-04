using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

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
                //creating init list of exchangeRATES
                List<ExchangeRate> exchangeRateList = new List<ExchangeRate>();
                var lines = GetDataFromCNB().Split('\n'); //after spliting to lines we will have each arrays element representing each line of txt file.

                //so we can iterate each line of datasource and create a list of ExchangeRates to show in the console later.
                for (int i = 0; i < lines.Length; i++)
                {
                    ProcessData(lines[i], currencies, exchangeRateList);
                }
                return exchangeRateList;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sorry, we can not display exchange rates. Error: {e.Message}");
                throw e;
            }

        }

        public string GetDataFromCNB()
        {
            try
            {
                //We take data from CNB txt file(s)
                //Need a webclient library to download those data
                using (var client = new WebClient())
                {
                    //there are o source of data. its better if we can join them and have one big source of exchange rates ^^
                    var exRates = client.DownloadString(ConfigurationManager.AppSettings["ApiUrl"]);
                    var exRatesOtherCurrencies = client.DownloadString(ConfigurationManager.AppSettings["ApiUrlOtherCurrencies"]);

                    return exRates + exRatesOtherCurrencies;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sorry, we can not get exchange rates from CNB. Error: {ex.Message}");
                return ex.Message;
            }

        }

        public void ProcessData(string line, IEnumerable<Currency> currencies, List<ExchangeRate> exchangeRateList)
        {
            //we only take data from lines with correct format data.Which means format,smth like  => Afghanistan|afghani|100|AFN|30.023
            if (IsCorrectFormat(line))
            {
                var elements = line.Split('|');
                Currency currencySource = new Currency(elements[3]);
                //if the currecy is included in currencies list
                if (currencies.ToList().Any(c => c.Code == currencySource.Code))
                {
                    int amount = Convert.ToInt32(elements[2]);
                    decimal rate = Convert.ToDecimal(elements[4]);
                    exchangeRateList.Add(new ExchangeRate(amount, currencySource, currencies.SingleOrDefault(c => c.Code == "CZK"), rate));
                }
            }
        }

        public bool IsCorrectFormat(string source)
        {
            //we can create a regex to check the format  ^^
            var regex = ConfigurationManager.AppSettings["FormatDataRegex"];
            var match = Regex.Match(source, regex);
            if (match.Success)
                return true;
            else
                return false;

        }
    }
}
