using Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services.Extensions
{
    public static class StringExtensions
    {
        public static ExchangeRate? ParseExchangeRate(this string line)
        {
            try
            {
                char separator = '|';

                string[] parsedDataArray = line.Split(separator);
                if (parsedDataArray.Length < 0)
                    return null;

                var cultureInfo = new CultureInfo("de-DE");

                decimal.TryParse(parsedDataArray[4], NumberStyles.Currency, cultureInfo, out var exchangeRate);
                return new ExchangeRate(new Currency(parsedDataArray[3]), new Currency("CZK"), exchangeRate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsToIgnoreLine(this string line)
        {
            try
            {
                string header = "země|měna|množství|kód|kurz";
                if (line.Equals(header))
                    return true;

                var regex = new System.Text.RegularExpressions.Regex(@"^([0-9]{2})[.]([0-9]{2})[.]([0-9]{4})");
                if (regex.IsMatch(line))
                    return true;

                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
