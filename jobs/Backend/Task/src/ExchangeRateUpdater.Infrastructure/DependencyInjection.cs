using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.Api.CzechNationalBankApi;
using ExchangeRateUpdater.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using System.Reflection;

namespace ExchangeRateUpdater.Infrastructure
{
    /// <summary>
    /// Provides extension methods for adding infrastructure services to the service collection.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds application services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CzechNationalBankConfiguration>(configuration.GetSection(CzechNationalBankConfiguration.SectionName));
            var cnbConfiguration = services.BuildServiceProvider()
                .GetRequiredService<IOptions<CzechNationalBankConfiguration>>().Value;

            services.AddHttpClient(CzechNationalBankConfiguration.SectionName, c =>
            {
                c.BaseAddress = new Uri(cnbConfiguration.ApiUrl);
            })
            .AddTypedClient(c => RestService.For<ICzechNationalBankApi>(c));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMemoryCache();

            services.AddKeyedTransient<IExchangeRatesRepository, ExchangeRatesRepository>(nameof(ExchangeRatesRepository));
            services.AddTransient<IExchangeRatesRepository, CacheExchangeRatesRepository>();

            return services;
        }
    }
}
