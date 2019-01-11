using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRateService
{
    public class CNBExchangeRateService : IExchangeRateService
    {
        private string URL;

        public CNBExchangeRateService(string dataUrl = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt")
        {
            URL = dataUrl;
        }

        /// <summary>
        /// Retrieve currency data from the CNB data source. This method will filter out all results from the 
        /// data source which are not requested in currencyCodes.
        /// </summary>
        /// <param name="currencyCodes">List of desired currency codes</param>
        /// <returns>A list of currency data for each of the desired currencies which is available from the source</returns>
        public IEnumerable<CurrencyData> GetExchangeRateData(IEnumerable<string> currencyCodes)
        {
            var client = new WebClient();
            var rates = new List<CurrencyData>();
            try
            {
                using (var stream = client.OpenRead(URL))
                using (var reader = new StreamReader(stream))
                {
                    // Skip the first two lines of the file (date and header lines)
                    reader.ReadLine();
                    reader.ReadLine();

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        rates.Add(ParseCurrencyData(line));
                    }

                    // CZK is the baseline currency used by the data source, so it
                    // needs to be manually added as a currency with an amount and value of 1
                    // this will only be used for calculated conversion rates
                    if (currencyCodes.Contains("CZK"))
                    {
                        rates.Add(new CurrencyData("CZK", 1.0m, 1));
                    }

                    // Filter the data so that the service only returns the desired currency information
                    return rates.Where(r => currencyCodes.Contains(r.CurrencyCode));
                }
            }
            catch(WebException ex)
            {
                var exception = new ApplicationException($"Data could not be retrieved from url: {URL}", ex);
                throw exception;
            }
        }

        /// <summary>
        /// Populates a new instance of the CurrencyData class given a line of data from the data
        /// source
        /// </summary>
        /// <param name="line">Line from the source text file</param>
        /// <returns>A populated currency data item</returns>
        private CurrencyData ParseCurrencyData(string line)
        {
            string[] values = line.Split('|');

            var currencyCode = values[3];
            var value = decimal.Parse(values[4].Replace(',' , '.'));
            var amount = int.Parse(values[2]);

            var result = new CurrencyData(currencyCode, value, amount);

            return result;
        }
    }
}
