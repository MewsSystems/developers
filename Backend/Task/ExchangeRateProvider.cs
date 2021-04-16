using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        public const string mRatesUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var lSourceCurrency = new Currency("CZK");

            var lResult = new List<ExchangeRate>();
            var lCnbRateList = await GetCnbRateList();

            foreach (var lCurrency in currencies)
            {
                var lCnbRate = lCnbRateList.FirstOrDefault(a => a.Code == lCurrency.Code);
                if (lCnbRate == null)
                    continue;

                var lRate = lCnbRate.Rate;

                // I´m avare that the specifications said not to calculate any exange rates but this is not exacly the 
                // situation that was written about in the summary above. JPY to CZK forexample would not make much sense 
                // otherwise. If i was sure that i was to ignore it it would be continue instead of the division

                if (lCnbRate.Ammount != 1)
                    lRate = lRate / lCnbRate.Ammount;


                var lExRate = new ExchangeRate(lSourceCurrency, new Currency(lCnbRate.Code), lRate);
                
                lResult.Add(lExRate);
            }

            return lResult;
        }

        public async Task<List<CnbRate>> GetCnbRateList() 
        {
            var lRateList = new List<CnbRate>();

            HttpClient lClient = new HttpClient();
            var lResponseString = await lClient.GetStringAsync(mRatesUrl);
            
            string[] lResponseLinesArr = lResponseString.Split(new[] { "\n" },StringSplitOptions.RemoveEmptyEntries).Skip(2).ToArray();

            foreach (var lLine in lResponseLinesArr)
            {
                lRateList.Add(ConvetToCnbRate(lLine));
            }

            return lRateList;
        }

        public CnbRate ConvetToCnbRate(string aLine) 
        {
            string[] lLine = aLine.Split('|');
            var lResult = new CnbRate
            {
                Country = lLine[0],
                Currency = lLine[1],
                Ammount = Int16.Parse(lLine[2]),
                Code = lLine[3],
                Rate = Decimal.Parse(lLine[4])
            };

            return lResult;
        }
    }
}
