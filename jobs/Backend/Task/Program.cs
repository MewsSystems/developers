using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
			var serviceProvider = services.RegisterServices().BuildServiceProvider();

			var rateProvider = serviceProvider.GetService<IExchangeRateProvider>();
			var rates = await rateProvider.GetExchangeRatesAsync(ExchangeRateSettings.Currencies);

			foreach (var rate in rates)
			{
				Console.WriteLine(rate.ToString());
			}

            Console.ReadLine();
        }

        static ServiceCollection RegisterServices(this ServiceCollection services)
        {
			services.AddSingleton<IExchangeRateClient, ExchangeRateClient>();
			services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
			services.AddSingleton<IRetryPolicy, RetryPolicy>();
			services.AddLogging(builder =>
			{
				builder.AddFile(ExchangeRateSettings.LoggingPath);
				builder.AddConsole();
				builder.AddDebug();
			});
			services.AddHttpClient("exchangeRates", c =>
			{
				c.BaseAddress = new Uri(ExchangeRateSettings.CnbExchangeRatesGetPath);
			});

			return services;
		}
	}
}
