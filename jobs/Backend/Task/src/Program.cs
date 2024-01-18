using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            var config = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .Build();

            var services = new ServiceCollection()
                .AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>()
                .AddPolicyHandler(GetRetryPolicy())
                .Services;

            // Make config injectable
            services.Configure<ApiSettings>(config.GetSection("apiSettings"));

            var serviceProvider = services.BuildServiceProvider();

            var provider = serviceProvider.GetService<IExchangeRateProvider>();

            try
            {
                var rates = (await provider.GetExchangeRates(currencies));

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

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // Could add a timeout policy here too, or a circuit breaker or any other useful policy,
            // but for now we'll just retry a few times in case of transient errors.
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(res => res.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
