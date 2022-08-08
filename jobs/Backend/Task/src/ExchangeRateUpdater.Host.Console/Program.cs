using System;
using System.Threading;
using Domain.UseCases;
using ExchangeRateUpdater.Host.Console.Configuration;
using ExchangeRateUpdater.Host.Console.Configuration.Logging;

namespace ExchangeRateUpdater.Host.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            const string ApplicationName = "ExchangeRateUpdater";
      
            var settingsConfiguration = Settings.GetSettingsConfiguration();
            var settings = Settings.From(settingsConfiguration, ApplicationName);
            
            var logger = SerilogConfiguration.Create(ApplicationName, settings);
            logger.Information("Logger created.");

            var servicesProviderConfiguration = new ServicesProviderConfiguration();
            servicesProviderConfiguration.SetupServices(settings, logger);

            var exchangeRatesSearcherService = servicesProviderConfiguration.GetExchangeRatesSearcherService();
            var cacheHelper = servicesProviderConfiguration.GetCacheHelper();
            var useCase = new GetExchangeRatesUseCase(exchangeRatesSearcherService, logger, settings.UseInMemoryCache, cacheHelper);
            
            useCase.ExecuteAsync();

            // [08/08/2022] AR - If we want to test the in memory cache mechanism
            //Thread.Sleep(TimeSpan.FromSeconds(2));
            //useCase.ExecuteAsync();

            System.Console.ReadLine();
        }
    }
}
