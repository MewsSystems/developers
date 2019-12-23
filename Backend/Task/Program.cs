using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IContainer container { get; set; }

        private static IEnumerable<Currency> currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };

        private static void RegisterDependencies()
        {
            //have to use Autofac since project uses .NET 4.5.2. Default Microsoft DI implementation requires at least ver. 4.6.1
            var builder = new ContainerBuilder();

            builder.RegisterType<HttpClient>();
            builder.RegisterType<CnbExchangeRateProvider>().As<IExchangeRateProvider>();
            builder.RegisterType<EcbExchangeRateProvider>().As<IExchangeRateProvider>();
            builder.RegisterType<ExchangeRateProviderFactory>().As<IExchangeRateProviderFactory>();
            builder.RegisterType<CnbClient>().As<ICnbClient>();

            container = builder.Build();
        }

        private static void OutputExchangeRates()
        {
            try
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    var factory = scope.Resolve<IExchangeRateProviderFactory>();
                    var provider = factory.GetExchangeRateProvider(ProviderName.CNB);
                    var rates = provider.GetExchangeRates(currencies);

                    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                    foreach (var rate in rates)
                    {
                        Console.WriteLine(rate.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }
        }

        public static void Main(string[] args)
        {
            RegisterDependencies();

            OutputExchangeRates();
            Console.ReadLine();
        }
    }
}
