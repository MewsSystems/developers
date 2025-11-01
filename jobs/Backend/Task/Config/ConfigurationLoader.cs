using System;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ExchangeRateUpdater.Config;

/// <summary>
///     Loads application configuration from environment variables.
///     Provides default values for currencies when not configured if applicable.
/// </summary>
public static class ConfigurationLoader
{
    public static AppConfiguration Load()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var currencies = configuration["CURRENCIES"];
        if (string.IsNullOrEmpty(currencies))
        {
            // Use Log.Logger at this point because DI is not set up yet
            Log.Logger.Information("CURRENCIES not configured. Using default currencies: USD, EUR, GBP.");
            currencies = "USD,EUR,GBP";
        }

        var providerTypeStr = configuration["PROVIDER_TYPE"];
        if (string.IsNullOrEmpty(providerTypeStr))
        {
            Log.Logger.Information("PROVIDER_TYPE not configured. Using default provider: CSV.");
            providerTypeStr = "CSV";
        }

        var exporterTypeStr = configuration["EXPORTER_TYPE"];
        if (string.IsNullOrEmpty(exporterTypeStr))
        {
            Log.Logger.Information("EXPORTER_TYPE not configured. Using default exporter: Console.");
            exporterTypeStr = "Console";
        }

        return new AppConfiguration
        {
            DailyRateUrl = configuration["DAILY_RATE_URL"] ?? "",
            Currencies = currencies,
            LogLevel = configuration["LOG_LEVEL"] ?? "Debug",
            CzkCurrencyCode = "CZK",
            DatabaseConnectionString = configuration["DATABASE_CONNECTION_STRING"] ?? "",
            ProviderType = Enum.Parse<RateProviderType>(providerTypeStr, true),
            ExporterType = Enum.Parse<RateExporterType>(exporterTypeStr, true)
        };
    }
}