using ExchangeRateUpdater.Domain.Config;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ExchangeRateUpdater
{
    public static class ServicesInstaller
    {
        public static ServiceProvider InstallServices()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();

            var services = new ServiceCollection();

            services.Configure<CacheConfig>(config.GetSection(nameof(CacheConfig)));
            services.Configure<CnbApiConfig>(config.GetSection(nameof(CnbApiConfig)));

            return services.AddLogging()
                           .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
                           .AddSingleton<IHttpClientService, HttpClientService>()
                           .BuildServiceProvider();
        }
    }
}
