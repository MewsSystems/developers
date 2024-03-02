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
    //TODO: we could read it directly from ISO 4217 api to get the latest updates.
    private static readonly string[] CurrenciesIso =
             { "AED", "AFN", "ALL", "AMD", "ANG", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOV", "BRL", "BSD", "BTN", "BWP", "BYR", "BZD", "CAD", "CDF", "CHE", "CHF", "CHW", "CLF", "CLP", "CNY", "COP", "COU", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EGP", "ERN", "ETB", "EUR", "FJD", "FKP", "GBP", "GEL", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "INR", "INR", "IQD", "IRR", "ISK", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MXN", "MXV", "MYR", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SLL", "SOS", "SRD", "SSP", "STD", "SVC", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USD", "USN", "UYI", "UYU", "UZS", "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XBA", "XBB", "XBC", "XBD", "XCD", "XDR", "XOF", "XPD", "XPF", "XPT", "XSU", "XTS", "XUA", "XXX", "YER", "ZAR", "ZAR", "ZAR", "ZMW", "ZWL" };


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
            var filteredCurrencies = Currencies.Where(x => CurrenciesIso.Contains(x.Code)).ToList();
            var rates = await provider.GetExchangeRates(filteredCurrencies, default);
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
