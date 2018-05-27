using System.Collections.Generic;
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
            List<ExchangeRate> return_list = new List<ExchangeRate>();

            // First we download a text file of exchange rates ČNB provides.
            // Unfortunately the data provided by ČNB is only CZK in relation to other currencies.
            // A good practice would be to download asynchronously, but considering the app has nothing to do while waiting for the download, lets not.

            using (var client = new WebClient())
            {
                client.DownloadFile("http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt", "rates.txt");
            }
            StreamReader rates_file = new StreamReader("rates.txt");


            // Now we read the file by lines, matching a pattern that fits "country name|currency name|count|ISO code|rate to CZK"...
            //
            // ...and pull data into 3 capture groups: 
            // count - weather we count for 1 of the currency or a different amout
            // iso   - international code for the currency
            // rate  - rate how many CZK for the "count" number of the other currency

            string line;
            while ((line = rates_file.ReadLine()) != null)
            {
                Regex pattern = new Regex("[a-zA-Z]+\\|[a-zA-Z]+\\|(?<count>1[0]*)\\|(?<iso>[A-Z]+)\\|(?<rate>[0-9]+,?[0-9]*)");
                Match match = pattern.Match(line);

                if (match.Success)
                {
                    // If pattern matches, first we search if the currency is listed as one we care about.
                    // and if so, we add a new ExchangeRate to the output list of ExchangeRate.
                    foreach (Currency curr in currencies)
                    {
                        if (curr.Code == match.Groups["iso"].Value)
                        {
                            return_list.Add(new ExchangeRate(new Currency("CZK"), curr, decimal.Parse(match.Groups["rate"].Value) / int.Parse(match.Groups["count"].Value)));
                            break;
                        }
                    }
                }
            }
            rates_file.Close();

            return return_list;
        }
    }
}
