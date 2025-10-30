using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.config;

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
            HttpTimeoutSeconds = int.TryParse(configuration["HTTP_TIMEOUT_SECONDS"], out var timeout) ? timeout : 30,
            Currencies = configuration["CURRENCIES"] ?? "",
            LogLevel = configuration["LOG_LEVEL"] ?? "Information"
        };
    }
}