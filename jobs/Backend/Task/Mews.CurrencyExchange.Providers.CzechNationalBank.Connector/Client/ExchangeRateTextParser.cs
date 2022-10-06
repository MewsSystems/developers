using System.Globalization;

namespace Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Client
{
    internal static class ExchangeRateTextParser
    {
        public static string[] SplitTextResponseIntoLines(string response)
        {
            var lines = response.Split("\n");
            return lines;
        }

        public static ProviderExchangeRate? ParseLine(string line, CultureInfo cultureInfo, short currencyIndex, short rateIndex)
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
