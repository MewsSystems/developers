using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.ConsoleApp.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater.ConsoleApp
{
    public static class Program
    {
        private static readonly IEnumerable<CurrencyModel> currencies = new[]
        {
            new CurrencyModel("USD"),
            new CurrencyModel("EUR"),
            new CurrencyModel("CZK"),
            new CurrencyModel("JPY"),
            new CurrencyModel("KES"),
            new CurrencyModel("RUB"),
            new CurrencyModel("THB"),
            new CurrencyModel("TRY"),
            new CurrencyModel("XYZ")
        };

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                services.ConfigureCustomServices();
            });

        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                using var serviceScope = host.Services.CreateScope();
                var services = serviceScope.ServiceProvider;
                var provider = services.GetRequiredService<ExchangeRateProviderController>();
                provider.GetExchangeRates(currencies).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

    }
}