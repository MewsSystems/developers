using ExchangeRateUpdater.Client.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Client.Bootstrap;

/// <summary>
/// Ideally we want to use IHttpClientFactory, so we can use connection pooling.
/// We can also use this bit to separate our DI stuff, add Polly for retry mechanisms or a failback caching mechanism.
/// Stops your Program.cs from being polluted.
///
/// Likewise you can set the BaseUrl here, using options pattern.
/// 
/// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0
/// </summary>
public static class ExchangeRateProviderBootstrap
{
    public static void AddCzechExchangeRateProvider(this IServiceCollection services)
    {
        services.AddHttpClient<IProviderClient, ProviderClient>();
    }
}