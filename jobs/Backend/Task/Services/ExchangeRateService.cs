using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Dtos;
using ExchangeRateUpdater.Providers;

namespace ExchangeRateUpdater.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        static IEnumerable<Currency> currencies = new[]
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
        
        readonly IExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateService(
            IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public void Execute()
        {
            try
            {
                var rates = _exchangeRateProvider.GetExchangeRates(currencies);

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