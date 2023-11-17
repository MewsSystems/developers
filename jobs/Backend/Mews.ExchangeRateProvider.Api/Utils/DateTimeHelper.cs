using System.Globalization;

namespace Mews.ExchangeRateProvider.Api.Utils
{
    /// <summary>
    /// check for provided date format, defaults to current date
    /// </summary>
    public static class DateTimeHelper
    {
        public const string ValidStringFormat = "yyyy-MM-dd";

        public static string ParseDateFormat(string? inputDate)
        {
            if (string.IsNullOrWhiteSpace(inputDate))
            {
                return DateTime.Today.ToString(ValidStringFormat);
            }
            DateTime result = new DateTime();
            if (DateTime.TryParseExact(inputDate, ValidStringFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result.ToString(ValidStringFormat);
            }
            return DateTime.Today.ToString(ValidStringFormat);
        }
    }
}
