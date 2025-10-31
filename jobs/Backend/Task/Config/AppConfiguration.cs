using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ExchangeRateUpdater.Models;
using Serilog.Events;

namespace ExchangeRateUpdater.Config;

public class AppConfiguration
{
    private static readonly HashSet<string> ValidCurrencyCodes = CultureInfo
        .GetCultures(CultureTypes.AllCultures)
        .Where(c => !c.IsNeutralCulture)
        .Select(culture =>
        {
            try
            {
                return new RegionInfo(culture.Name).ISOCurrencySymbol;
            }
            catch
            {
                return null;
            }
        })
        .Where(x => x != null)
        .Distinct()
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public string DailyRateUrl { get; init; }

    public string Currencies { get; init; }

    public string LogLevel { get; init; }

    public string CzkCurrencyCode { get; init; }

    public string DatabaseConnectionString { get; init; }

    public RateProviderType ProviderType { get; init; }

    public RateExporterType ExporterType { get; init; }

    public IEnumerable<Currency> GetCurrencies()
    {
        if (string.IsNullOrWhiteSpace(Currencies))
            return Enumerable.Empty<Currency>();

        return Currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(code => code.Trim().ToUpperInvariant())
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Select(code => new Currency(code));
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(DailyRateUrl))
            throw new InvalidOperationException(
                "DAILY_RATE_URL environment variable is required and cannot be empty");

        if (!Enum.IsDefined(typeof(LogEventLevel), LogLevel))
        {
            var validLevels = string.Join(", ", Enum.GetNames(typeof(LogEventLevel)));
            throw new InvalidOperationException($"Invalid log level: {LogLevel}. Valid options are: {validLevels}");
        }

        if (!string.IsNullOrWhiteSpace(Currencies))
        {
            var invalidCodes = Currencies
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(code => code.Trim().ToUpperInvariant())
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Where(code => !ValidCurrencyCodes.Contains(code))
                .ToList();

            if (invalidCodes.Any())
                throw new InvalidOperationException(
                    $"Invalid currency code(s): {string.Join(", ", invalidCodes)}. Must be valid ISO 4217 codes.");
        }

        if (!Enum.IsDefined(typeof(RateProviderType), ProviderType))
        {
            var validTypes = string.Join(", ", Enum.GetNames(typeof(RateProviderType)));
            throw new InvalidOperationException(
                $"Invalid provider type: {ProviderType}. Valid options are: {validTypes}");
        }

        if (!Enum.IsDefined(typeof(RateExporterType), ExporterType))
        {
            var validTypes = string.Join(", ", Enum.GetNames(typeof(RateExporterType)));
            throw new InvalidOperationException(
                $"Invalid exporter type: {ExporterType}. Valid options are: {validTypes}");
        }
    }

    public LogEventLevel GetLogLevel()
    {
        return Enum.TryParse<LogEventLevel>(LogLevel, true, out var level) ? level : LogEventLevel.Information;
    }
}