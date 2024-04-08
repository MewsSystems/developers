using ExchangeRateProvider.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateProvider
{
    public static class ExchangeRateProviderExtensions
    {
        /// <summary>
        /// Adds services related to exchange rate provider to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> with the added exchange rate provider services.</returns>
        public static IServiceCollection AddExchangeRateProvider(this IServiceCollection services)
        {
            services.AddHttpClient<ICnbHttpClient, CnbHttpClient>();
            services.AddScoped<IExchangeRateProvider, CnbExchangeRateProvider>();
            return services;
        }
    }
}