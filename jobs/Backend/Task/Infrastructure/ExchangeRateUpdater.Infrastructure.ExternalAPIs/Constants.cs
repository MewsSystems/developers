namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs;

internal static class Constants
{
    internal const string HttpClientOptionsSectionName = "ExchangeRateProvider:Client";
    internal const string HttpClientResilience = "ExchangeRateProvider-Http-Resilience";
    internal const string CachingOptionsSectionName = "ExchangeRateProvider:Caching";
}