using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ExchangeRateUpdater
{
    internal static class Startup
    {
        public static IServiceProvider SetupContainer()
        {
            var config = GetConfiguration();

            var services = new ServiceCollection();

            var cnbClientOptions = config.GetSection("CnbClient").Get<CnbClient.Options>();
            services.AddSingleton(cnbClientOptions);

            var exchangeRateCacheOptions = config.GetSection("ExchangeRateCache").Get<ExchangeRateCache.Options>();
            services.AddSingleton(exchangeRateCacheOptions);

            services.AddSingleton<ICnbClient, CnbClient>();
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddSingleton<IExchangeRateCache, ExchangeRateCache>();

            services.AddLogging(l => l.AddConsole());

            services.AddMemoryCache();


            return services.BuildServiceProvider();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
            return builder.AddJsonFile("appsettings.json").Build();
        }
    }
}