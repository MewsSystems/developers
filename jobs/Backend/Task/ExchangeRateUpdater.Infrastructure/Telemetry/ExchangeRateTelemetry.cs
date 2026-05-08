using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace ExchangeRateUpdater.Infrastructure.Telemetry;

public static class ExchangeRateTelemetry
{
    public static readonly ActivitySource ActivitySource = new("ExchangeRateUpdater");
    public static readonly Meter Meter = new("ExchangeRateUpdater.Metrics");

    // Business metrics
    public static readonly Counter<long> ExchangeRateRequests =
        Meter.CreateCounter<long>("exchange_rate_requests_total", "Total number of exchange rate requests");

    public static readonly Counter<long> CacheHits =
        Meter.CreateCounter<long>("cache_hits_total", "Total number of cache hits");

    public static readonly Counter<long> CacheMisses =
        Meter.CreateCounter<long>("cache_misses_total", "Total number of cache misses");

    public static readonly Counter<long> CacheOperations =
        Meter.CreateCounter<long>("cache_operations_total", "Total number of cache operations");

    public static readonly Histogram<double> ExchangeRateDuration =
        Meter.CreateHistogram<double>("exchange_rate_duration_seconds", "Duration of exchange rate operations");

    public static readonly Histogram<double> CacheOperationDuration =
        Meter.CreateHistogram<double>("cache_operation_duration_seconds", "Duration of cache operations");
}
