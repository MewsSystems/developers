using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Src;
using ExchangeRateUpdater.Src.Cnb;
using ExchangeRateUpdater.Src.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Globalization;

namespace ExchangeRateUpdater;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        DateOnly requestedDate = ParseRequestedDate(args);

        var services = new ServiceCollection();
        ConfigureLogging(services);
        ConfigureOptions(services);
        await ConfigureCachingAsync(services);
        ConfigureAppServices(services);

        using ServiceProvider provider = services.BuildServiceProvider();
        ILogger logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("Main");

        try
        {
            var rateProvider = provider.GetRequiredService<IExchangeRateProvider>();
            List<ExchangeRate> rates = await rateProvider.GetAsync(requestedDate, cts.Token);

            string effective = rates[0].ValidFor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.WriteLine($"CNB Exchange Rates (ValidFor={effective}) — {rates.Count} items:");
            foreach (ExchangeRate r in rates.OrderBy(x => x.SourceCurrency))
                Console.WriteLine(r.ToString());

            return 0;
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Operation canceled.");
            return 130;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not retrieve exchange rates.");
            return 1;
        }
    }

    private static DateOnly ParseRequestedDate(string[] args)
    {
        if (args.Length == 1 && DateOnly.TryParse(args[0], out DateOnly d))
            return d;
        return DateOnly.FromDateTime(DateTime.UtcNow);
    }

    private static void ConfigureLogging(ServiceCollection services)
    {
        services.AddLogging(b => b
            .AddSimpleConsole(o =>
            {
                o.SingleLine = true;
                o.TimestampFormat = "HH:mm:ss ";
                o.IncludeScopes = true;
            })
            .SetMinimumLevel(LogLevel.Information));
    }

    private static void ConfigureOptions(ServiceCollection services)
    {
        services.Configure<CnbOptions>(_ => { });
    }

    private static async Task ConfigureCachingAsync(ServiceCollection services)
    {
        string conn = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? "127.0.0.1:6379";

        IConnectionMultiplexer? mux = null;
        try
        {
            var cfg = ConfigurationOptions.Parse(conn);
            cfg.AbortOnConnectFail = false;
            cfg.ConnectTimeout = 2000;
            cfg.SyncTimeout = 2000;

            mux = await ConnectionMultiplexer.ConnectAsync(cfg);
            if (mux.IsConnected)
            {
                Console.WriteLine($"INFO: Connected to Redis at {string.Join(",", cfg.EndPoints)}");
                services.AddStackExchangeRedisCache(o =>
                {
                    o.ConnectionMultiplexerFactory = () => Task.FromResult<IConnectionMultiplexer>(mux);
                    o.InstanceName = "cnb:";
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WARN: Could not connect to Redis at '{conn}' ({ex.Message}). Falling back to in-memory cache.");
        }

        if (mux is null || !mux.IsConnected)
        {
            Console.WriteLine("WARN: Redis unavailable; using in-memory distributed cache.");
            services.AddDistributedMemoryCache();
        }
    }

    private static void ConfigureAppServices(ServiceCollection services)
    {
        services.AddSingleton<IExchangeRateCache, ExchangeRateCache>();
        services.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>();
    }
}
