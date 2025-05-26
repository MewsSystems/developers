namespace ExchangeRateUpdater.Common.Constants
{
    public static class AppConstants
    {
        // Cache keys
        public const string DailyRatesCacheKey = "CnbApiRatesDict";
        public const string MonthlyRatesCacheKey = "CnbApiMonthlyRatesDict";
       // Keyed services
        public const string DailyRatesKeyedService = "daily";
        public const string MonthlyRatesKeyedService = "monthly";
    }
}