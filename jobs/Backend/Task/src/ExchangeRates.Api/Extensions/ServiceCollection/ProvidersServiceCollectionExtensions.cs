using ExchangeRates.Api.Infrastructure.Providers;

namespace Microsoft.Extensions.DependencyInjection;

public static class ProvidersServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRatesApiProviders(this IServiceCollection services)
    {
        return services
                .AddSingleton<IExchangeRatesProvider, CnbExchangeRatesProvider>();
    }
}
