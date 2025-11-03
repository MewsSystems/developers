using System.Collections.Generic;
using ExchangeRateUpdater.Models;
using Serilog.Events;

namespace ExchangeRateUpdater.Config;

/// <summary>
///     Interface for application configuration settings.
/// </summary>
public interface IAppConfiguration
{
    string DailyRateUrl { get; }
    string Currencies { get; }
    string LogLevel { get; }
    string CzkCurrencyCode { get; }
    string DatabaseConnectionString { get; }
    RateProviderType ProviderType { get; }
    RateExporterType ExporterType { get; }

    IEnumerable<Currency> GetCurrencies();
    void Validate();
    LogEventLevel GetLogLevel();
}