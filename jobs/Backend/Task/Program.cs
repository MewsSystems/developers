using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRateUpdater.Dtos;

namespace ExchangeRateUpdater
{
    public class Program
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
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddTransient<IApiClientService, ApiClientService>()
            .AddTransient<IDeserializationService, DeserializationService>()
            .AddTransient<IExchangeRateProviderService, ExchangeRateProviderService>()
            .BuildServiceProvider();

            try
            {
                var clientInstance = serviceProvider.GetService<IApiClientService>();
                var responseText = await clientInstance.ConsumeEndpoint("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");

                var deserializatorInstance = serviceProvider.GetService<IDeserializationService>();
                var responseDeserialized = deserializatorInstance.Deserialize<ExchangeRate>(responseText);

                var exchangeRateSerivceInstance = serviceProvider.GetService<IExchangeRateProviderService>();
                
                var rates =  exchangeRateSerivceInstance.GetExchangeRatesAsync(currencies, responseDeserialized);
                var exchangeRatesDto = rates.Select(a => new ExchangeRateDto(new Currency(a.Code), new Currency("CZK"), Convert.ToDecimal(a.ExchangeRate)));


                Console.WriteLine($"Successfully retrieved {exchangeRatesDto.Count()} exchange rates:");
                foreach (var exchangeRates in exchangeRatesDto)
                {
                    Console.WriteLine(exchangeRates.ToString());
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
