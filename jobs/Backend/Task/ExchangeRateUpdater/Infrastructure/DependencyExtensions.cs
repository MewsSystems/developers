using System.Net.Http;
using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.DataSource.Cnb;
using ExchangeRateUpdater.DataSources.RefreshSchedule;
using ExchangeRateUpdater.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure;

public static class DependencyExtensions
{
    public static IServiceCollection AddExchangeRatesProvider(this IServiceCollection services)
    {
        services.AddLogging(builder => builder.AddConsole());
        services.AddMemoryCache();
        services.AddSingleton<HttpClient>();

        var config = new ConfigurationBuilder()
                        .AddJsonFile("appSettings.json")
                        .Build()
                        .Get<AppConfiguration>(); // TODO: return default configuration if can't load from json;
        services.AddKeyedSingleton(Constants.CnbServiceKey, config.CnbExchangeRateLoaderConfig);

        services.AddTransient<CnbRateConverter>();
        services.AddTransient<IRequestHandler, HttpRequestHandler>();
        services.AddTransient<IDateTimeService, LocalDateTimeService>();
        services.AddTransient<IRefreshScheduleFactory, RefreshScheduleFactory>();
        services.AddTransient<IExchangeRateLoader, CnbExchangeRateLoader>();
        services.AddTransient<ExchangeRateProvider>();

        return services;
    }
}
