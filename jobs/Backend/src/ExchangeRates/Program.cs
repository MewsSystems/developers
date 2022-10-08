using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRates.Contracts;
using ExchangeRates.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRates
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
            new Currency("XYZ"),
            new Currency("IDR")
        };

        public static async Task Main(string[] args)
        {             
            var configuration = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	            .AddEnvironmentVariables()	            
	            .Build();

            var serviceProvider = new ServiceCollection()
                .AddCustomizedLogging()
                .AddCustomizedOptions(configuration)
                .AddHttpClient()
                .AddCustomizedClients()
                .AddCustomizedParsers()
                .AddCustomizedExchangeRateProviders()
                .AddCustomizedCultureProviders()
				.BuildServiceProvider();

            try
            {
                Console.WriteLine("Enter the day in the format 'd.m.yyyy', you want to aquire the exchange rate for.");
                Console.WriteLine("Leave empty for the most recent date.");
                var exchangeRateDate = Console.ReadLine();
                DateOnly? day = string.IsNullOrWhiteSpace(exchangeRateDate)
                    ? null
                    : DateOnly.Parse(
                        exchangeRateDate,
                        serviceProvider.GetService<ICnbCultureProvider>().GetCultureInfo());

                var tokenSource = new CancellationTokenSource();
                // Uncomment to simulate token cancellation
				//tokenSource.Cancel();

				var provider = serviceProvider.GetService<ICnbExchangeRateProvider>();				
				var rates = await provider.GetExchangeRatesAsync(currencies, day, tokenSource.Token);

                Console.WriteLine(string.Empty);
				Console.WriteLine($"Successfully retrieved {rates.Length} exchange rates:");
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
    }
}
