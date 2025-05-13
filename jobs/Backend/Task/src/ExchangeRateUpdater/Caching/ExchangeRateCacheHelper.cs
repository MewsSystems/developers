namespace ExchangeRateUpdater.Caching;

public static class ExchangeRateCacheHelper
{
    public static class Cnb
    {
        public static class CommonRates
        {
            public const string CacheKey = "CnbCommonRates";
            public static DateTimeOffset GetExpiry()
            {
                DateTime today = DateTime.Today;
                DateTime expiry = today.AddDays(1).Date;
                return new DateTimeOffset(expiry);
            }
        }
        public static class UncommonRates
        {
            public const string CacheKey = "CnbUncommonRates";
            public static DateTimeOffset GetExpiry()
            {
                DateTime today = DateTime.Today;
                DateTime firstOfNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
                return new DateTimeOffset(firstOfNextMonth);
            }
        }
    }
}
