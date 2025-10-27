using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfiguration = configuration
                .GetSection(Constants.ApiConfiguration)
                .Get<ApiConfiguration>();

            services.AddSingleton(apiConfiguration!);
            services.AddScoped<IDateTimeSource, DateTimeSource>();
            services.AddScoped<ExchangeRateProvider>();
        }
    }
}
