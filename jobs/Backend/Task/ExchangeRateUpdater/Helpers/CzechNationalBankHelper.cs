using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Helpers
{
    public static class CzechNationalBankHelper
    {
        /// <summary>
        /// Convert from a format given by the CNB to a generic Third Party Exchange Rate format.
        /// CNB format here:
        /// https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates/
        /// </summary>
        /// <param name="data"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<ThirdPartyExchangeRate> ConvertToThirdPartyExchangeRates(string data, char separator = '|')
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            var rates = new List<ThirdPartyExchangeRate>();

            var lines = data.Split('\n');

            // Skip the first two lines that contain the date and the rate format
            for (var i = 2; i < lines.Length; i++)
            {
                var line = lines[i];

                // We only want a usable exchange rate in a specific format,
                // otherwise skip it
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var segments = line.Split(separator);

                var rate = new ThirdPartyExchangeRate
                {
                    Country = segments[0],
                    Currency = segments[1],
                    Code = segments[3]
                };

                // Safely parse amount to decimal, skipping the record if unable
                if (!decimal.TryParse(segments[2], out var amount))
                {
                    continue;
                }
                else
                {
                    rate.Amount = amount;
                }

                // Safely parse rate to decimal, skipping the record if unable
                if (!decimal.TryParse(segments[4], out var rateValue))
                {
                    continue;
                }
                else
                {
                    rate.Rate = rateValue;
                }

                rates.Add(rate);
            }

            return rates;
        }
    }
}
