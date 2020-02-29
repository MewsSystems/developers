using System;
using System.Globalization;

namespace ExchangeRateUpdater.Utils
{
    internal class Parsers
    {
        public static decimal Parse(string value)
        {
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
            { 
                return decimalValue; 
            }
            throw new ApplicationException($"Parsing error. Value '{value}' cannot be parse to decimal.");
        }       
    }
}
