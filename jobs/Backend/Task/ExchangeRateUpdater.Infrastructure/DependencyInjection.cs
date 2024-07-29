using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Infrastructure.HttpClients;
using ExchangeRateUpdater.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddExchangeRateUpdaterInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient<IExchangeRateRepository, CzechNationalBankClient>();

            services.AddScoped<ICurrencySourceRepository, CurrencySourceRepository>();

            return services;
        }
    }
}
