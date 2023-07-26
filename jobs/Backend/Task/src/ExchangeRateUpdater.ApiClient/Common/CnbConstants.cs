
namespace ExchangeRateUpdater.ApiClient.Infrastructure
{
    internal static class CnbConstants
    {
        public const string DATE_FORMAT = "yyyy-MM-dd";
        public static string ExchangeRatesDaily(DateTime date, string language) =>  $"cnbapi/exrates/daily?date={date.ToString(DATE_FORMAT)}&lang={language}";
    }
}
