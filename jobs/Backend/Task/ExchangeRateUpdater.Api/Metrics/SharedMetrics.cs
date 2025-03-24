using System.Diagnostics.Metrics;

namespace ExchangeRateUpdater.Api.Metrics;

public static class SharedMetrics
{
    private static readonly Meter ExchangeRatesMeter = new("ExchangeRatesHandler", "1.0.0");

    public static readonly Counter<int> ExchangeRatesCounter = ExchangeRatesMeter.CreateCounter<int>(
        "exchange_rates.count",
        description: "Counts the number of exchange rates provided by the API");
}