using Mews.ExchangeRate.Storage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.ExchangeRate.Storage.DistributedCache.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddExchangeRateStorage(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddScoped<IExchangeRateQueryRepository, ExchangeRateStorageQueryRepository>();
            services.AddScoped<IExchangeRateCommandRepository, ExchangeRateStorageCommandRepository>();

            return services;
        }
    }
}
