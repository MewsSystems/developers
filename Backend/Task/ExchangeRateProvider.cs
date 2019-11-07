using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Returns exchange rates among the specified currencies that are defined by the source
        /// <param name="currencies"/>A list of allowed currencies</param>
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string source = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
            Currency localCurrency = new Currency("CZK");
            var client = new WebClient();
            Stream response = client.OpenRead(source); // If an exception occurs it will be handeled by the Program class
            StreamReader reader = new StreamReader(response); // Pack the stream to read textual data line by line

            List<ExchangeRate> exchangeList = new List<ExchangeRate>(); // An exchange list to be returned
            ExchangeRate exchangeRate = null;
            Dictionary<string, Currency> dictionary = currencies.ToDictionary(i => i.Code); // Build a dictionary using currency codes

            string str; // A string to be parsed
            while ((str = reader.ReadLine()) != null) // While we can read the input file
            {
                if((exchangeRate = RetrieveExchangeRate(str, localCurrency, dictionary)) != null) // If we got a correct ExchangeRate object
                {
                    exchangeList.Add(exchangeRate);
                }
            }
            reader.Close();
            response.Close();
            return exchangeList;
        }

        /// <summary>
        /// Parses a line and tries to build an ExchangeRate object out of the data gotten in case the source currency is in the given list 
        /// </summary>
        /// <param name="line">A line to be parsed</param>
        /// <param name="targetCurrency">Target currency for an ExchangeRate instance to be created</param>
        /// <param name="permittedCurrencies">List of permitted currencies</param>
        /// <returns>An ExchangeRate object if the line argument is a correct line, null otherwise</returns>
        private ExchangeRate RetrieveExchangeRate(string line, Currency targetCurrency, Dictionary<string, Currency> permittedCurrencies)
        {
            try // Try to parse the line
            {
                char delimiter = '|';
                string[] tokens = line.Split(delimiter);
                if (!permittedCurrencies.ContainsKey(tokens[3])) // If the source currency is not in the list
                {
                    return null;
                }
                permittedCurrencies.TryGetValue(tokens[3], out Currency sourceCurrency);
                int amount = Convert.ToInt16(tokens[2]);
                decimal rate = Convert.ToDecimal(tokens[4].Replace(',', '.')); // Format the value - decimal point instead of a comma
                return new ExchangeRate(sourceCurrency, targetCurrency, rate / amount);
            } catch
            {
                return null; // The line cannot be parsed
            }
        }
    }
}