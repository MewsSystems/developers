using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Mews.ExchangeRateUpdater.Infrastructure.Logging;

public class TraceIdEnricher : ILogEventEnricher
{
    public const string TraceIdPropertyName = "TraceId";
    
    // Custom enricher to add TraceId
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? "N/A";
        var traceIdProperty = propertyFactory.CreateProperty(TraceIdPropertyName, traceId);
        logEvent.AddPropertyIfAbsent(traceIdProperty);
    }
}