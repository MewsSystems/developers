using System.Diagnostics.Metrics;

namespace ExchangeRateUpdater.Api.Configuration;

public static class Telemetry
{
    private static readonly Meter ExchangeRatesMeter = new("ExchangeRateMetrics");

    public static readonly Counter<int> ExchangeRatesCount = ExchangeRatesMeter.CreateCounter<int>(
        "exchange_rates_count",
        description: "Counts the number of exchange rates provided by the API");
    
    public const string Currency = "Currency";
}