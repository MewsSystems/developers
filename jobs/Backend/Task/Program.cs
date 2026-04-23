using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static readonly IEnumerable<Currency> currencies =
        [
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        ];

        public static async Task Main(string[] args)
        {
            try
            {
                //Configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                //Dependency Injection
                var services = new ServiceCollection();
                ConfigureServices(services, configuration);
                var serviceProvider = services.BuildServiceProvider();
              
                var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRatesAsync(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                    Console.WriteLine(rate.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.AddHttpClient<CNBExchangeRateApiProvider>();
            services.AddTransient<IExchangeRateProvider, CNBExchangeRateApiProvider>();

            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddConfiguration(configuration.GetSection("Logging"));
            });
        }
    }
}
