using System;
using System.Globalization;

namespace ExchangeRateUpdater.Util
{
    public static class Extensions
    {
        public static decimal ToDecimal(this string decimalStr)
        {
            var culture = CultureInfo.CreateSpecificCulture("cs-CZ");
            if (decimal.TryParse(decimalStr, NumberStyles.Any, culture, out var parsed))
            {
                return parsed;
            }
            throw new FormatException($"Failed to parse '{decimalStr}' to decimal.");
        }
    }
}
