using System;
using System.Diagnostics.Metrics;

namespace ExchangeRateUpdater.Infrastructure.Observability
{
    internal sealed class Metrics : IDisposable
    {
        public const string ExchangeRateUpdateMeterName = "ExchangeRateUpdate.Metrics";

        private readonly Meter meter;

        public Metrics(IMeterFactory meterFactory)
        {
            meter = meterFactory.Create(ExchangeRateUpdateMeterName, "1.0.0");
            RetryCounter = meter.CreateCounter<long>("httpclient_retry_count", description: "Number of retries");
            CircuitBreakerOpened = meter.CreateCounter<long>("circuit_breaker_opened_count", description: "Number of times the circuit breaker opened");
            CircuitBreakerReset = meter.CreateCounter<long>("circuit_breaker_reset_count", description: "Number of times the circuit breaker reset");
        }

        public Counter<long> RetryCounter { get; }

        public Counter<long> CircuitBreakerOpened { get; }

        public Counter<long> CircuitBreakerReset { get; }

        public void Dispose()
        {
            meter.Dispose();
        }
    }
}
