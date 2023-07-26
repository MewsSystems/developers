using ExchangeRateUpdater.Models.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Features;
using System.Threading.Tasks;
using ExchangeRateUpdater.Features.Services;
using ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1;
using ExchangeRateUpdater.Features.Configuration;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<CurrencyModel> currenciesModel = new[]
        {
            new CurrencyModel("USD"),
            new CurrencyModel("EUR"),
            new CurrencyModel("CZK"),
            new CurrencyModel("JPY"),
            new CurrencyModel("KES"),
            new CurrencyModel("RUB"),
            new CurrencyModel("THB"),
            new CurrencyModel("TRY"),
            new CurrencyModel("XYZ")
        };
        public static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            DoConfigure(serviceCollection);

            try
            {
                var serviceProvider = serviceCollection.BuildServiceProvider();

                var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();
                var rates = await exchangeRateService.GetExchangeRates(currenciesModel);
                var rates2 = await exchangeRateService.GetExchangeRates(currenciesModel);

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


        private static void DoConfigure(IServiceCollection services)
        {
            services.AddExchangeRateUpdaterFeature(opts =>
            {
                opts.Timeout = TimeSpan.Parse(AppSettingsProvider.CnbClientOptions.Timeout);
                opts.RetryOptions = RetryOptions.Default;
                opts.BaseUrl = AppSettingsProvider.CnbClientOptions.BaseUrl;
            });
        }
    }
}
