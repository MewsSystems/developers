using System;
using System.Collections.Generic;
using System.Globalization;
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
            string dailyRateUrl = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
            string dailyRateRaw;
            string[] dailyRateSplitted;
            Currency sourceCurr = new Currency("CZK");
            CultureInfo localCulture = CultureInfo.CreateSpecificCulture("cs-CZ");
            NumberStyles style = NumberStyles.AllowDecimalPoint;

            // using webclient get raw data as one string
            using (WebClient webClient = new WebClient())
            {
                dailyRateRaw = webClient.DownloadString(dailyRateUrl);
            }

            // if data not found, return empty list
            if (dailyRateRaw == null)
            {
                return Enumerable.Empty<ExchangeRate>();
            }

            // create new Dictionary of ExchangeRate, for fast searching
            Dictionary<string, ExchangeRate> fullRateList = new Dictionary<string, ExchangeRate>();
            List<ExchangeRate> requestedRateList = new List<ExchangeRate>();

            // data found, split them to individual items
            dailyRateSplitted = dailyRateRaw.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // loop all items and check if is valid
            // O(n)
            foreach (string dailyRateItem in dailyRateSplitted)
            {
                // if item valid continue parsing
                if (IsValid(dailyRateItem))
                {
                    // split by pipe
                    string[] dailyRateItemValues = dailyRateItem.Split('|');

                    // amount 
                    Int16.TryParse(dailyRateItemValues[2], out Int16 count);

                    // rate
                    Decimal.TryParse(dailyRateItemValues[4], style, localCulture, out decimal rate);

                    // target currency
                    Currency targetCurr = new Currency(dailyRateItemValues[3]);

                    // create new dictionary value
                    fullRateList.Add(dailyRateItemValues[3], new ExchangeRate(sourceCurr, targetCurr, rate / count));
                }
            }

            // O(n)
            foreach (var currency in currencies)
            {
                // O(1)
                if (fullRateList.TryGetValue(currency.Code, out ExchangeRate tmpExRate))
                {
                    requestedRateList.Add(tmpExRate);
                }
            }

            return requestedRateList;
        }

        private bool IsValid(string dailyRateItem)
        {
            // valid string contains: 
            // 1. string with special czech chars; 
            // 2. pipe sign; 
            // 3. same as number 1.
            // 4. pipe sign; 
            // 5. any power of 10
            // 6. pipe sign; 
            // 7. 3 uppercase chars
            // 8. pipe sign; 
            // 9. double with comma as separator, rounded to 3 decimal places
            string template = @"[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽ\s]+\|[a-zA-ZáčďéěíňóřšťůúýžÁČĎÉĚÍŇÓŘŠŤŮÚÝŽ\s]+\|1[0]*\|[A-Z]{3}\|[0-9]+,[0-9]{3}";
            return Regex.IsMatch(dailyRateItem, template);
        }
    }
}
