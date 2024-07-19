using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExternalVendors.CzechNationalBank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) => builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
                .ConfigureServices((context, services) =>
                {
                    services.Configure<CzechNationalBankSettings>(context.Configuration.GetSection("CNB_API"));
                    services.AddTransient<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
                    services.AddTransient<IExchangeRateClient, CzechApiClient>();
                    services.AddMemoryCache();
                    services.AddHttpClient("ExchangeRateApi", (provider, client) =>
                    {
                        var settings = provider.GetRequiredService<IOptions<CzechNationalBankSettings>>().Value;
                        client.BaseAddress = settings.BASE_URL;
                    });
                })
                .Build();

            var provider = ActivatorUtilities.CreateInstance<CzechNationalBankExchangeRateProvider>(host.Services);
            
            try
            {
                var config = host.Services.GetRequiredService<IConfiguration>();
                var codes = config.GetSection("DESIRED_CURRENCIES").Get<List<string>>();
                var currencies = codes.Select(code => new Currency(code)).ToList();
                var rates = await provider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.ToString()}'.");
            }

            Console.ReadLine();
        }
    }
}
