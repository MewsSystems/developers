using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services.AddScoped<IExchangeRateProviderRepository, CzechNationalBankApiClient>();
        }
    }
}
