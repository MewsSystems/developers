
namespace ExchangeRateUpdater.ApiClient.Common
{
    public enum Language
    {
        CN, EN
    }

    internal static class CnbConstants
    {
        public const string DATE_FORMAT_yyyy_MM_dd = "yyyy-MM-dd";

        public static string ExchangeRatesDaily(DateTime date, string language) => $"cnbapi/exrates/daily?date={date.ToString(DATE_FORMAT_yyyy_MM_dd)}&lang={language}";

    }
}
