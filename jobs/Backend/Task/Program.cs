using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ExchangeRateUpdater
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var services = CreateServices();
            IApplication app = services.GetRequiredService<IApplication>();
            await app.Execute();
        }

        private static ServiceProvider CreateServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
                .AddSingleton<IExchangeRateService, ExchangeRateService>()
                .AddSingleton<IApplication, Application>()
                .AddHttpClient()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }

    public interface IApplication
    {
        Task Execute();
    }

    public class Application : IApplication
    {
        private IExchangeRateService _exchangeRateService;

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

        public Application(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task Execute()
        {
            try
            {
                var rates = await _exchangeRateService.GetExchangeRates(currencies);

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
    }
}
