using ExchangeRates.Api.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsServiceCollectionExtensions
{
    public static IServiceCollection AddExchageRateApiOptions(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddOptions()
            .ConfigureSettings<CnbHttpClientOptions>(config, "HttpClients:Cnb");
    }

    private static IServiceCollection ConfigureSettings<T>(this IServiceCollection services, IConfiguration config, string sectionName) where T : class
    {
        return services.Configure<T>(config.GetSection(sectionName));
    }
}