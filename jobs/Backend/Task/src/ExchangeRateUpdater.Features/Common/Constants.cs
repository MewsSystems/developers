using ExchangeRateUpdater.Models.Domain;

namespace ExchangeRateUpdater.Features.Common
{
    internal class Constants
    {
        public static readonly string CacheName = "ExchangeRateUpdaterCache";

        public const string CACHE_DATE_FORMAT = "yyyy-MM-dd";

        public static readonly Currency CZK = new("CZK");
    }
}
