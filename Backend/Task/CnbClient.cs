using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ExchangeRateUpdater
{
    public class CnbClient
    {
        private const string RatesUrl =
            "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        
        public static IEnumerable<RateEntry> GetRateEntries()
        {
            string[] rateRows;
            using (var client = new WebClient{Encoding = Encoding.UTF8})
                rateRows = client.DownloadString(RatesUrl).Split(new []{'\n'}, StringSplitOptions.RemoveEmptyEntries).Skip(2).ToArray();
            return rateRows.Select(RateEntry.Parse).ToArray();
        }
    }
}