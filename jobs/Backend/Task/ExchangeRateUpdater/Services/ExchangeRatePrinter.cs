using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeEntities;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Services
{
	public class ExchangeRatePrinter : IExchangeRatePrinter
    {
        public void Print(IEnumerable<ExchangeRate> exchangeRates)
        {
            Console.WriteLine($"Successfully retrieved {exchangeRates.Count()} exchange rates:");

            foreach (var rate in exchangeRates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
    }
}

