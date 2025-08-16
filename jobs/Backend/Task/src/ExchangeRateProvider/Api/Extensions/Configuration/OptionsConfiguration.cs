using ExchangeRateUpdater.Infrastructure.Options;
using ExchangeRateUpdater.Infrastructure.Options.Clients;

namespace Microsoft.Extensions.DependencyInjection;

public static class OptionsConfiguration
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration config)
    {
        services
            .ConfigureOptions<CzechNationalBankApiClientOptions>(config, "ExternalApis:CzechNationalBank")
            .ConfigureOptions<CnbExchangeRatesUpdaterOptions>(config, "HostedServices:CnbExchangeRatesUpdater")
            .ConfigureOptions<ExchangeRatesOptions>(config, "ExchangeRates");

        return services;
    }

    private static IServiceCollection ConfigureOptions<T>(this IServiceCollection services, IConfiguration config, string configPath) where T : class
    {
        services
            .AddOptions<T>()
            .Bind(config.GetSection(configPath))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}
