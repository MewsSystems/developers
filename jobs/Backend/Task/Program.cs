using Application.Services.Implementations;
using Application.Services.Interfaces;
using CNBClient.Implementations;
using CNBClient.Interfaces;
using Data.CNBGateway;
using Domain.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public static class Program
    {
     


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
                var rates = rateService.GetExchangeRates();

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
