using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

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
        /// 
        string newLine = "\n";
        char delimeter = '|';
        string dataSourceUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=[dd]+[MMM]+[yyyy]";
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
           
                var result = new List<ExchangeRate>();
                var rateUrl = dataSourceUrl;
                var dt = DateTime.Today;
                rateUrl = rateUrl.Replace("[dd]", dt.Day.ToString()).Replace("[MMM]", GetAbbrMonthString(dt.Month)).Replace("[yyyy]", dt.Year.ToString());
                var czkCurr= new Currency("CZK");

                var webData = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rateUrl);

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        webData = reader.ReadToEnd();
                    }
                }
                catch(Exception ex)
                {
                    // Internet connection/file availability 
                    throw ex;
                }

                // Validate webData has content
                if(string.IsNullOrEmpty(webData))
                {
                    throw new Exception("Source data is empty!");
                }

                var currLines = webData.Split(new string[] { newLine }, StringSplitOptions.None);
                
                if(currLines.length<3)
                {
                    throw new Exception("No data found!");
                }

                // ignore header information by starting at line 3
                for (int i = 2; i < currLines.Length; i++)
                {
                    var linecontent = currLines[i].Split(delimeter);
                    if (linecontent.Length != 5)
                    {
                        continue;
                    }

                    var amt = decimal.Parse(linecontent[2]);
                    var currCode = linecontent[3];
                    var rate = decimal.Parse(linecontent[4]);
                    if (currencies.Any(o => o.Code.Equals(currCode, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        result.Add(new ExchangeRate(new Currency(currCode), czkCurr, rate / amt));
                    }
                }

                return result;// Enumerable.Empty<ExchangeRate>();
           
        }

        public string GetAbbrMonthString(int monthIndex)
        {
            var months = new[]
            {
                "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"
            };

            return months[monthIndex - 1];
        }
    }
}
