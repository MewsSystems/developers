using ExchangeRateUpdater.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.BusinessLogic.Interfaces;
using ExchangeRateUpdater.BusinessLogic.Implementations;
using ExchangeRateUpdater.BusinessLogic.Cnb.Services.Implementations;
using ExchangeRateUpdater.BusinessLogic.Models.Constants;
using ExchangeRateUpdater.BusinessLogic.Models.Cnb.Constants;
using ExchangeRateUpdater.Clients.Cnb.Implementations;
using ExchangeRateUpdater.Clients.Cnb.Interfaces;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using IHost host = GetConfiguredHost(args);

            try
            {
                var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
                var currenciesCodes = host.Services.GetService<IConfiguration>().GetRequiredSection(Constants.SettingsCurrenciesSection).Get<IEnumerable<string>>();
                if(currenciesCodes?.Any() == true)
                {
                    Console.WriteLine($"Retrieved {currenciesCodes.Count()} currencies codes to search.");

                    var rates = provider.GetExchangeRates(currenciesCodes.Select(x => new Currency(x)), CnbConstants.GetDefaultCurrency());

                    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                    foreach (var rate in rates)
                    {
                        Console.WriteLine(rate.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        private static IHost GetConfiguredHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {

                services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
                services.AddScoped<IExchangeService, CnbExchangeService>();
                services.AddScoped<IExchangesServicesFactory, ExchangesServicesFactory>();
                services.AddScoped<ICnbExchangeClient, CnbExchangeClient>();

                services.Configure<IConfiguration>(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build());
                services.AddHttpClient();
            }).Build();
        }
    }


}
