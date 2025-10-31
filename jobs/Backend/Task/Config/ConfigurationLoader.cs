using System;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Config;

public static class ConfigurationLoader
{
    public static AppConfiguration Load()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        return new AppConfiguration
        {
            DailyRateUrl = configuration["DAILY_RATE_URL"] ?? "",
            Currencies = configuration["CURRENCIES"] ?? "",
            LogLevel = configuration["LOG_LEVEL"] ?? "Debug",
            CzkCurrencyCode = "CZK",
            ProviderType =
                Enum.Parse<RateProviderType>(Environment.GetEnvironmentVariable("PROVIDER_TYPE") ?? "Csv", true),
            ExporterType =
                Enum.Parse<RateExporterType>(Environment.GetEnvironmentVariable("EXPORTER_TYPE") ?? "Console", true)
        };
    }
}