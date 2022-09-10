
using Application.Services.Implementations;
using Application.Services.Interfaces;
using CNBClient.Implementations;
using CNBClient.Interfaces;
using Data.CNBGateway;
using Domain.Core;
using Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class Program
    {


        public static IEnumerable<Currency> currencies = new[]
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

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var rateService = serviceProvider.GetService<IExchangeRateProvider>();

            try
            {
                Console.WriteLine("Starting Application");
                rateService.LoadData();
                var rates = rateService.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates)
                {
                    Console.WriteLine(rate.ToString());
                }

                var exchangeRate = rateService.Convert(new Domain.Model.Currency("PUT"),new Domain.Model.Currency("USD"), 10);

                if(exchangeRate is null)
                {
                    Console.WriteLine("Invalid currency inserted");
                    Console.ReadLine();
                }

                Console.WriteLine(exchangeRate.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            //setup our DI
            services
                .AddScoped<IExchangeRateProvider, ExchangeRateProvider>()
                .AddScoped<ICNBGateway, CNBGateway>()
                .AddScoped<ICNBExchageRateUpdater, CNBExchageRateUpdater>()
                .BuildServiceProvider();
        }
    }
}
