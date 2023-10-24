using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Repository;
using ExchangeRateUpdater.Repository.Abstract;
using ExchangeRateUpdater.Service;
using ExchangeRateUpdater.Service.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	public static class Program
    {
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

        public static async Task Main(string[] args)
        {
			var serviceProvider = ConfigureServices();
			var provider = serviceProvider.GetRequiredService<IExchangeRateService>();
			try
            {
				var rates = await provider.GetExchangeRates(currencies);

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

		private static ServiceProvider ConfigureServices()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

			var configuration = builder.Build();
			Console.WriteLine(Directory.GetCurrentDirectory());
			var services = new ServiceCollection();

			services.AddSingleton<IConfiguration>(configuration);
			services.AddTransient<ICzechNationalBankRepository, CzechNationalBankRepository>();
			services.AddTransient<IExchangeRateService, ExchangeRateService>();

			return services.BuildServiceProvider();
		}
	}
}
