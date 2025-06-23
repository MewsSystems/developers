using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateUpdater.Application.Const
{
    [ExcludeFromCodeCoverage]
    public static class CacheConstants
    {
        public static string ExchangeRateKey(string providerCode, DateTime? date)
        {
            return $"PROV_{providerCode}_DATE_{date.GetValueOrDefault().ToString("yyyy-MM-dd")}_KEY";
        }
    }
}
