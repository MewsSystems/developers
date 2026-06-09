using ExchangeRateUpdater.Cache;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var configuration = BuildConfiguration();
                var currencies = configuration.GetSection("Exchange:Currencies").Get<string[]>().Select(currency => new Currency(currency));

                var serviceProvider = BuildServiceProvider(configuration);

                var provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();

                var rates = await provider.GetExchangeRatesAsync(currencies, new Currency(currencies.First().Code));

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        static IConfiguration BuildConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            return configuration;
        }

        static IServiceProvider BuildServiceProvider(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.Configure<ServiceConfiguration>(configuration.GetSection("Exchange"));

            services.AddScoped<ExchangeRateProvider>();

            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddSingleton(typeof(ICacheService<,>), typeof(CacheService<,>));

            services.AddHttpClient<IExchangeService, ExchangeService>(client =>
            {
                client.BaseAddress = new Uri(configuration["Exchange:CzechNationalBankExchangeApi"]);
            });

            return services.BuildServiceProvider();
        }
    }
}
