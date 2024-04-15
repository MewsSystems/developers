using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{

    public class Worker : BackgroundService
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        private static IEnumerable<Currency> currencies = new[]  {
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

        public Worker(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var rates = await _exchangeRateProvider.GetExchangeRates(currencies, stoppingToken);

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
