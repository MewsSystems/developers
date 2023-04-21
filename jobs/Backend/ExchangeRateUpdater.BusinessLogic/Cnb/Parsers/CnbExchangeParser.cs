using ExchangeRateUpdater.BusinessLogic.Models;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace ExchangeRateUpdater.BusinessLogic.Cnb.Parsers
{
    public static class CnbExchangeParser
    {
        private const char LineBreak = '\n';

        public static decimal? ParseCnbFxExchangeRate(string separatorKey, string resultFxExchange, int unitsIndex, int valueIndex)
        {
            ArgumentNullException.ThrowIfNull(separatorKey);
            ArgumentNullException.ThrowIfNull(resultFxExchange);

            var resultFxCnbByLines = resultFxExchange.Split(LineBreak).Where(x => x.Split(separatorKey).Length > 1);
            var foundFxLine = resultFxCnbByLines.Any() ? resultFxCnbByLines.Last() : null;

            var value = foundFxLine?.Split(separatorKey).Length > valueIndex ? foundFxLine?.Split(separatorKey)[valueIndex] : null;
            var units = foundFxLine?.Split(separatorKey).Length > unitsIndex ? foundFxLine?.Split(separatorKey)[unitsIndex] : null;
            return GetDecimalValue(foundFxLine, value, units);
        }

        public static decimal? ParseCnbExchangeRate(string separatorKey, Currency currency, string result, int unitsIndex, int codeIndex, int valueIndex)
        {
            ArgumentNullException.ThrowIfNull(separatorKey);
            ArgumentNullException.ThrowIfNull(currency);
            ArgumentNullException.ThrowIfNull(result);

            var resultCnbByLines = result.Split(LineBreak);
            var foundLine = resultCnbByLines.FirstOrDefault(x =>
            {
                var line = x.Split(separatorKey);
                if (line.Length > 1)
                    return x.Split(separatorKey)[codeIndex] == currency.Code;
                else
                    return false;
            });

            var value = foundLine?.Split(separatorKey).Length > valueIndex ? foundLine?.Split(separatorKey)[valueIndex] : null;
            var units = foundLine?.Split(separatorKey).Length > unitsIndex ? foundLine?.Split(separatorKey)[unitsIndex] : null;
            return GetDecimalValue(foundLine, value, units);
        }

        private static decimal? GetDecimalValue(string foundLine, string exchangeValue, string unitsString)
        {
            if (!string.IsNullOrEmpty(foundLine) && 
                decimal.TryParse(exchangeValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value) &&
                decimal.TryParse(unitsString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal units)
                )
            {
                
                return value / units;
            }

            return null;
        }
    }
}
