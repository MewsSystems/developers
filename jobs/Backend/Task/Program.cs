using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHost(args);
            var config = host.Services.GetRequiredService<IConfiguration>();
            var specifiedCurrencies = GetSpecifiedCurrencies(config);
            var provider = host.Services.GetRequiredService<IExchangeRateProvider>();

            await FetchExchangeRates(specifiedCurrencies, provider);
        }
        
        private static IHost CreateHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient<IBankClient, BankClient>(client =>
                    {
                        client.BaseAddress = new Uri(context.Configuration["ExchangeRatesApiURL"]);
                    });
                    services.AddMemoryCache();
                    services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
                })
                .Build();
        }

        private static IEnumerable<Currency> GetSpecifiedCurrencies(IConfiguration config)
        {
            return config.GetRequiredSection("SpecifiedCurrencies")
                .Get<string[]>()
                .Select(code => new Currency(code));
        }

        private static async Task FetchExchangeRates(IEnumerable<Currency> specifiedCurrencies, IExchangeRateProvider provider)
        {
            try
            {
                var rates = await provider.GetExchangeRatesAsync(specifiedCurrencies);

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
