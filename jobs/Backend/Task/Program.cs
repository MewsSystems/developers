using ExchangeRateUpdater.ExchangeRateAPI;
using ExchangeRateUpdater.ExchangeRateAPI.CBNClientApi;
using ExchangeRateUpdater.ExchangeRateAPI.ExchangeRateProvider;
using ExchangeRateUpdater.ExchangeRateAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
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
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient<ICBNClientApi, CBNClientApi>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
            })
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            builder.Services.AddTransient<IExchangeRateProvider, CBNExchangeRateProvider>();
            builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

            builder.Services.Configure<Settings>(builder.Configuration.GetSection(""));

            var app = builder.Build();

            try
            {
                var provider = app.Services.GetRequiredService<IExchangeRateProvider>();

                var rates = await provider.GetExchangeRates(currencies);

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

        public static WebApplication ConfigureServices(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient<ICBNClientApi, CBNClientApi>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
            })
            .AddPolicyHandler(
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            builder.Services.AddTransient<IExchangeRateProvider, CBNExchangeRateProvider>();
            builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

            builder.Services.Configure<Settings>(builder.Configuration.GetSection(""));

            var app = builder.Build();

            return app;
        }
    }
}
