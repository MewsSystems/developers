using System;
using System.Linq;
using System.Net.Http;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                }) .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient<CzechNationalBankExchangeRateProvider>();
                })
                .Build();
            
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            
            string baseUrl = configuration["ExchangeRateProvider:BaseUrl"];
            string[] currencyCodes = configuration.GetSection("Currencies").Get<string[]>();
            var currencies = currencyCodes.Select(code => new Currency(code)).ToArray();
            
            
            var httpClientFactory = host.Services.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            
            try
            {
                var provider = new CzechNationalBankExchangeRateProvider(httpClient, baseUrl);
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

            Console.ReadLine();
        }
    }
}
