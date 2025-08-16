/*
   System:         N/A
   Component:      ExchangeRateUpdater
   Filename:       ExchangeRateUtils.cs
   Created:        31/07/2023
   Original Author:Tom Doran
   Purpose:        Util class with helper methods for Exchange rate provision
 */

using System.Collections.Generic;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater.Helpers
{
    public class ExchangeRateParser
    {
        // I have made the assumption that this project will only provide FX rates 
        // for CZK - extending this to include other currencies would be a fairly simple
        // change though.
        private const string BASE_CCY = "CZK";

        /// <summary>
        /// Parses API call response to list of ExchangeRate objects, with CZK (Czech Koruna) 
        /// as base currency.
        /// </summary>
        public static List<ExchangeRate> ParseResponseToKorunaRates(string contentString)
        {
            var korunaExchangeRates = new List<ExchangeRate>();

            var newLineDelimeter = "\n";
            var columnDelimeter = "|";

            // parse response string row by row
            var rowsOfContent = contentString.Split(newLineDelimeter);
            foreach (var row in rowsOfContent)
            {
                // if this row is not one of the first 2 rows (which are a header & the column names)
                if (row.Contains(columnDelimeter) && !row.Contains("Country|Currency"))
                {
                    var columns = row.Split(columnDelimeter);
                    // Columns are parsed in the following order:
                    // Country (0), Currency (1), Amount (2), Code (3), Rate (4)
                    var amount = int.Parse(columns[2]);
                    var code = columns[3];
                    var rate = decimal.Parse(columns[4]);

                    korunaExchangeRates.Add(BuildExchangeRate(code, rate, amount));
                }
            }
            return korunaExchangeRates;
        }

        private static ExchangeRate BuildExchangeRate(string code, decimal rate, int amount)
        {
            // standardise amount for 1 CZK, e.g. if exchange rate is given for 100 CZK - calculate for 1
            var exchangeRate = rate / amount;

            // create target currency & source (will always be CZK from this dataset)
            var sourceCcy = new Currency(BASE_CCY);
            var targetCcy = new Currency(code);

            // return exchange rate with information
            return new ExchangeRate(sourceCcy, targetCcy, exchangeRate);
        }
    }
}