using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public sealed class ExchangeRatePrinter : IExchangeRatePrinter
{
    public void PrintRates(IEnumerable<ExchangeRate> rates)
    {
        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        foreach (var rate in rates)
        {
            Console.WriteLine(rate);
        }
        Console.ReadKey();
    }
}
