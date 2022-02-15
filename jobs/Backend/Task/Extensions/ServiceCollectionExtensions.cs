using ExchangeRateUpdater.ExchangeProviders;
using ExchangeRateUpdater.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeProviders(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ExchangeProviderUrl"];
            services.AddSingleton<IExchangeRateProvider>(provider => 
            new CNBProvider(
                url,
                provider.GetService<IFileProvider>(), 
                provider.GetService<ICNBParser>(), 
                provider.GetService<ILoggerFactory>().CreateLogger<CNBProvider>()));

            services.AddSingleton<ICNBParser, CNBParser>();

            return services;
        }

        public static IServiceCollection AddFileProviders(this IServiceCollection services, IConfiguration configuration)
        {
            var filename = configuration["LatestFilename"];
            services.AddSingleton<IFileProvider>(provider => 
            new FileProvider(
                filename,
                provider.GetService<ILoggerFactory>().CreateLogger<FileProvider>()));

            return services;
        }
    }
}
