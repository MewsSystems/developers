using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSerilogLogging(builder.Configuration);

            builder.Services.AddHttpClient<ICzechNationalBankApiClient, CzechNationalBankApiClient>(httpClient => 
            {
                var bankApiSettings = builder.Configuration
                    .GetSection("CzechNationalBankApi")
                    .Get<CzechNationalBankApiSettings>();

                httpClient.BaseAddress = new Uri(bankApiSettings.BaseUrl);
            })
            .AddTransientHttpErrorPolicy(policyBuilder => 
            {
                return policyBuilder.WaitAndRetryAsync(6, 
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            });
            builder.Services.AddTransient<ExchangeRateProvider>();

            var app = builder.Build();

            await RunTest(app);
        }

        private static async Task RunTest(IHost app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var provider = scope.ServiceProvider.GetRequiredService<ExchangeRateProvider>();
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
