using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Presentation.Cli;

public static class ExchangeRatePrinter
{
    public static void Print(ExchangeRate rate)
    {
        Console.WriteLine($"{rate.SourceCurrency.Code}/{rate.TargetCurrency.Code}={rate.Value}");
    }

    public static void Print(IEnumerable<ExchangeRate> rates)
    {
        foreach (var rate in rates)
        {
            Print(rate);
        }
    }
}