namespace ExchangeRateUpdater.Infrastructure.HttpClients.Config;

public static class UrlSettings
{
    public static class CnbApiOperations
    {
        public static string DailyExchangeRatesUrl => "/cnbapi/exrates/daily";
    }
}
