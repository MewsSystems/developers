using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;

namespace ExchangeRateUpdater.config;

public class AppConfiguration
{
    public string CnbDailyRateUrl { get; set; }
    
    public int HttpTimeoutSeconds { get; set; }
    
    public string Currencies { get; set; }
    
    public string LogLevel { get; set; }

    public IEnumerable<Currency> GetCurrencies()
    {
        if (string.IsNullOrWhiteSpace(Currencies))
            return Enumerable.Empty<Currency>();

        return Currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(code => code.Trim())
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Select(code => new Currency(code));
    }
    
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(CnbDailyRateUrl))
        {
            throw new InvalidOperationException("CNB_DAILY_RATE_URL environment variable is required and cannot be empty");
        }
    
        if (!Enum.IsDefined(typeof(LogEventLevel), LogLevel))
        {
            var validLevels = string.Join(", ", Enum.GetNames(typeof(LogEventLevel)));
            throw new InvalidOperationException($"Invalid log level: {LogLevel}. Valid options are: {validLevels}");
        }
    }
    
    public LogEventLevel GetLogLevel()
    {
        return Enum.TryParse<LogEventLevel>(LogLevel, true, out var level) ? level : LogEventLevel.Information;
    }
}