using CnbServiceClient.Extensions;
using ExchangeRateTool.Factories;
using ExchangeRateTool.Interfaces;
using ExchangeRateTool.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateTool.Extensions
{
	public static class ServiceCollectionExtension
	{
        public static IServiceCollection AddExchangeRateTool(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCnbServiceClient(configuration);

            services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddScoped<IExchangeRateFactory, ExchangeRateFactory>();
            services.AddScoped<IExrateFilterService, ExrateFilterService>();

            return services;
        }
    }
}

