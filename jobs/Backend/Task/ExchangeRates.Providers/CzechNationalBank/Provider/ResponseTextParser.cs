using ExchangeRate.Datalayer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Providers.CzechNationalBank.Provider
{
    public static class ResponseTextParser
    {
        public static string[] SplitTextResponseIntoLines(string response)
        {
            var lines = response.Split("\n");
            return lines;
        }

        public static ProviderExchangeRate? ParseLine(string line, CultureInfo cultureInfo, int currencyIndex, int rateIndex)
        {
            if (!line.Contains('|'))
                return null;

            var columns = line.Split('|');

            var currency = columns[currencyIndex];
            if (!decimal.TryParse(columns[rateIndex], NumberStyles.Any, cultureInfo, out decimal rate))
            {
                return null;
            }

            var providerExchangeRate = new ProviderExchangeRate(currency, rate);

            return providerExchangeRate;
        }
    }
}
