using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddOptions<CnbApiOptions>()
                .Bind(builder.Configuration.GetSection(CnbApiOptions.SectionName))
                .ValidateOnStart();

            builder.Services.AddTransient<ITimeProvider, SystemTimeProvider>();
            builder.Services.AddTransient<IExchangeRateProvider, CnbExchangeRateProvider>();
            builder.Services.AddHttpClient<ICnbApiClient, CnbApiClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<CnbApiOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(options.RequestTimeoutSeconds);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            var host = builder.Build();

            try
            {
                var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRatesAsync(currencies);

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
