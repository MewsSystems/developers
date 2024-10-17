using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
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

            builder.Services.AddSerilogLogging(builder.Configuration);

            builder.Services.AddHttpClient(HttpClientNames.CzechNationalBankApi, httpClient => 
            {
                var baseUrl = builder.Configuration.GetValue<string>("CzechNationalBankApiBaseUrl");

                httpClient.BaseAddress = new Uri(baseUrl);
            })
            .AddTransientHttpErrorPolicy(policyBuilder => 
            {
                return policyBuilder.WaitAndRetryAsync(6, 
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            });
            
            builder.Services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
                .AddSingleton<IExchangeRateApiClientFactory, ExchangeRateApiClientFactory>();

            var app = builder.Build();

            await RunTest(app);
        }

        private static async Task RunTest(IHost app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var provider = scope.ServiceProvider.GetRequiredService<IExchangeRateProvider>();
                var rates = await provider.GetExchangeRatesAsync(currencies, WellKnownCurrencyCodes.CZK);

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
