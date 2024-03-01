using ExchangeRates.App.Caching;
using ExchangeRates.App.Provider;
using ExchangeRates.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRates.App;

public sealed class Program
{
    private static readonly IEnumerable<Currency> Currencies = new[]
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
        try
        {
            var builder = CreateHostBuilder(args).Build();
            var provider = builder.Services.GetRequiredService<IExchangeRateProvider>();
            await builder.StartAsync();
            var rates = await provider.GetExchangeRates(Currencies, default);

            var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
            Console.WriteLine($"Successfully retrieved {exchangeRates.Count()} exchange rates:");
            foreach (var rate in exchangeRates)
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


    static IHostBuilder CreateHostBuilder(string[] args)
    {
        var redisUrl = Environment.GetEnvironmentVariable("REDIS_URL") ?? "localhost:6379";
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((hostContext, services) =>
        {

            services.AddCnbClient();
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new ConfigurationOptions { AbortOnConnectFail = false, EndPoints = new() { redisUrl } };
                options.InstanceName = "ExchangeRates";
            });
            services.AddScoped<IClock>(_ => SystemClock.Instance);
            services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
            services.Configure<RedisOptions>(x => x.ExpireAt = TimeSpan.Parse(hostContext.Configuration["Redis:ExpireAt"]!));
            services.AddScoped<ICacheService, RedisCacheService>();
        });
    }
}
