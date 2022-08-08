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
            var useCase = new GetExchangeRatesUseCase(exchangeRatesSearcherService, logger);
            
            useCase.ExecuteAsync();

            System.Console.ReadLine();
        }
    }
}
