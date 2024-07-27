using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddExchangeRateUpdaterInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient<IExchangeRateRepository, CzechNationalBankClient>();

            return services;
        }
    }
}
