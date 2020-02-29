using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ExchangeRateUpdater.Configuration
{
    public class Startup
    {
        public static void Init(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
                                                  
            var logger = services.AddLogging()
                .BuildServiceProvider()
                .GetService<ILoggerFactory>()
                .AddLog4Net("log4net.config")
                .CreateLogger("ExchangeRateUpdater");

            services.AddSingleton<ICnbExchangeRateService>(
                provider => new CnbExchangeRateService(
                    new CnbExchangeRateFixingConfiguration(configuration),
                    logger));
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
        }
    }
}
