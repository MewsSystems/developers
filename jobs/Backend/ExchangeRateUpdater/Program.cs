using System;
using ExchangeRateUpdater.CnbProxy;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Utilities.IoC;
using ExchangeRateUpdater.Utilities;
using ExchangeRateUpdater.Utilities.Logging;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IAppLogger _logger;

        public static void Main(string[] args)
        {
            InitializeContainer();

            try
            {
                var provider = IoC.Container.GetInstance<IExchangeRateProvider>();

                provider.RetrieveExchangeRatesAsync();
            }
            catch (Exception e)
            {
                _logger.Fatal($"Fatal error: '{e.Message}'");
            }

            Console.ReadLine();
        }

        private static void InitializeContainer()
        {
            try
            {
                IoC.Init();
                IoC.AddRegistry(new ExchangeRateUpdaterUtilitiesRegistry());
                IoC.AddRegistry(new ExchangeRateUpdaterRegistry());
                IoC.AddRegistry(new ExchangeRateUpdaterCnbProxyRegistry());

                _logger = IoC.Container.GetInstance<IAppLogger>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Missing IoC instance: '{e.Message}'");
            }
        }
    }
}
