using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.Infrastructure.ExchangeRateVendors;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        
        private static IEnumerable<Currency> _currencies = new[]
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

        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
               
            // Add services to the container.
            builder.Services.AddController();
            
            builder.Services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });

            builder.Services.AddOptions().AddServices();
            
            


            builder.Services.AddOptions<CurrencyApiOptions>()
                .Configure(opts =>
                {
                    opts.BaseUri = builder.Configuration["CurrencyApi:BaseUrl"];
                    opts.ApiKey = builder.Configuration["CurrencyApi:ApiKey"];
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();
            
            try
            {
                var provider = new ExchangeRateProvider();
                var rates = provider.GetExchangeRates(_currencies);

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
