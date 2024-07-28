using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Infrastructure.Repositories;
using ExchangeRateUpdater.Infrastructure.Services;
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
