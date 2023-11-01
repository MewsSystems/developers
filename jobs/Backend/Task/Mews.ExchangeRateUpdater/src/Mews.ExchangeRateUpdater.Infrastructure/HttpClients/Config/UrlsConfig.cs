namespace Mews.ExchangeRateUpdater.Infrastructure.HttpClients.Config;

/// <summary>
/// Url configuration for API calls.
/// </summary>
public static class UrlsConfig
{
    /// <summary>
    /// Urls for Czech National Bank Api operations.
    /// </summary>
    public class CzechNationalBankApiOperations
    {
        /// <summary>
        /// Url for getting the daily exchange rates.
        /// </summary>
        /// <returns></returns>
        public static string GetDailyExchangeRates() => "/cnbapi/exrates/daily";
    }
}
