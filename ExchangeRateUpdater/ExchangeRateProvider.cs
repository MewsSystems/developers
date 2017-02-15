using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        //I used fixer.io api as current and historical foreign exchange rates published by the European Central Bank,
            //which is one of the best banks for currency operation.
        private const string _service = "http://api.fixer.io/latest?base="; 


        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> excRates = new List<ExchangeRate>();
            using (WebClient wc = new WebClient())
            {
                foreach (Currency cur in currencies)
                {
                    string json = String.Empty;
                    try
                    {
                        json = wc.DownloadString(_service + cur.Code);
                    }
                    catch (Exception e)
                    {
                        //TODO: we need to create log file with mesages about service call exception where we should put currency name and exception text
                    }
                    if (json != string.Empty)
                    { 
                        JObject excRate = JObject.Parse(json);
                        foreach (JToken rate in excRate["rates"].Children().ToList())
                        {
                            string[] rateArray = rate.ToString().Replace("\"", "").Split(':');
                            if (rateArray.Length == 2)
                            {
                                ExchangeRate er = new ExchangeRate(cur, new Currency(rateArray[0]), decimal.Parse(rateArray[1]));
                                excRates.Add(er);
                            }
                        }
                    }
                }
            }
            return excRates;
        }
    }
}
