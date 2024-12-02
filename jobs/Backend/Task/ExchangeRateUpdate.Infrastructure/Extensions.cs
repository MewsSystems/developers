using ExchangeRateUpdater.Infrastructure.Apis;
using ExchangeRateUpdater.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure;

public static class Extensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IExchangeRateApi, CnbExchangeRateApi>();
        services.AddHttpClient<IExchangeRateApi, CnbExchangeRateApi>((httpClient) =>
        {
            httpClient.BaseAddress = new Uri(configuration.GetSection("ExchangeRateApiUrl")["CzechNationalBank"] ?? "");
        });
    }
}
