using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        CultureInfo enCulture = new CultureInfo("en-US");

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string result = null;

            //We declared a model (ExternalCurrency) of CNB data source currency model. We used it as list below.

            List<ExternalCurrency> listEc = new List<ExternalCurrency>();

            //Data source gives only CZK therefore, we select target currency as CZK
            Currency target = new Currency("CZK");

            //Declared datasource url with dynamic date. Values are changing every day.
            string dataSource = String.Format(@"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date={0}", DateTime.Today.ToString("dd.MM.yyyy"));

            using (WebClient client = new WebClient())
            {
                result = client.DownloadString(dataSource);
            }
            //We download the data from CNB as string using by webclient.

            string[] lines = result.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            //String data split by lines and removed empty lines (Normally it doesn't has empty line but we have to check this)

            foreach (var line in lines.Skip(2)) //We skipped first two line because first line has date and second line has title. 
            {
                string[] item = line.Split('|'); //Currency properties are seperated with "|" in data source we splitted it by "|"

                ExternalCurrency ec = new ExternalCurrency()
                {
                    Country = item[0],
                    Currency = item[1],
                    Amount = Convert.ToDecimal(item[2], enCulture),
                    Code = item[3],
                    Rate = Convert.ToDecimal(item[4], enCulture)
                };

                listEc.Add(ec);
                //We set every data to our ExternalCurrency model and added to list.
            }

            var listER = listEc
                .Where(l => currencies.Any(c => c.Code == l.Code))
                .Select(l => new ExchangeRate(new Currency(l.Code), target, l.Rate));

            // we have matched the currencies given as source in the our data source list. If match success it returns value as ExchangeRate output.

            return listER;
        }
    }
}
