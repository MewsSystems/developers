using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExternalVendors.CzechNationalBank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHost();
            var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
            var config = host.Services.GetRequiredService<IConfiguration>();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                var codes = config.GetSection("DESIRED_CURRENCIES").Get<List<string>>();
                var currencies = codes.Select(code => new Currency(code)).ToList();
                var rates = await provider.GetExchangeRates(currencies);
                logger.LogInformation($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    logger.LogInformation(rate.ToString());
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Could not retrieve exchange rates: '{e}'.");
            }

            Console.ReadLine();
        }

        private static IHost CreateHost()
        {
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureServices((context, services) =>
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
            });
            return builder.Build();
        }
    }
}