using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<ExchangeRateFetcher>();
                }).UseConsoleLifetime();
 
            var host = builder.Build();
 
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
 
                try
                {
                    var myService = services.GetRequiredService<ExchangeRateFetcher>();
                    var result = await myService.Run();
                    var provider = new ExchangeRateProvider(result);
                    var rates = provider.GetExchangeRates(currencies);

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
            }

            Console.ReadLine();
        }
    }
}