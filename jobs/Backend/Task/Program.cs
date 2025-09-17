using ExchangeRateUpdater.Contracts;   // IExchangeRateProvider
using ExchangeRateUpdater.Src.Cnb;     // CnbApiExchangeRateProvider, CnbOptions
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Globalization;

namespace ExchangeRateUpdater;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        using CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

        DateOnly date = args.Length == 1 && DateOnly.TryParse(args[0], out DateOnly parsed)
            ? parsed
            : DateOnly.FromDateTime(DateTime.UtcNow);

        ServiceCollection services = new ServiceCollection();

        services.AddLogging(b => b.AddSimpleConsole(o =>
        {
            o.SingleLine = true;
            o.TimestampFormat = "HH:mm:ss ";
            o.IncludeScopes = true;
        }).SetMinimumLevel(LogLevel.Information));

        // Use defaults for CNB behavior (publish time, retries, cache TTL, etc.)
        services.Configure<CnbOptions>(_ => { });

        // Prefer local Docker Redis on 6379; env var can override.
        string redisConn = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? "127.0.0.1:6379";

        IConnectionMultiplexer? mux = null;
        try
        {
            ConfigurationOptions cfg = ConfigurationOptions.Parse(redisConn);
            cfg.AbortOnConnectFail = false;
            cfg.ConnectTimeout = 2000; // ms
            cfg.SyncTimeout = 2000;    // ms

            mux = await ConnectionMultiplexer.ConnectAsync(cfg);
            if (mux.IsConnected)
            {
                Console.WriteLine($"INFO: Connected to Redis at {string.Join(",", cfg.EndPoints)}");
                services.AddStackExchangeRedisCache(options =>
                {
                    // Reuse the established connection
                    options.ConnectionMultiplexerFactory = () =>
                        Task.FromResult<IConnectionMultiplexer>(mux);
                    options.InstanceName = "cnb:";
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WARN: Could not connect to Redis at '{redisConn}' ({ex.Message}). Falling back to in-memory cache.");
        }

        if (mux is null || !mux.IsConnected)
        {
            Console.WriteLine("WARN: Redis unavailable; using in-memory distributed cache.");
            services.AddDistributedMemoryCache();
        }

        // Register the provider (JSON API + Polly + cache inside the provider)
        services.AddHttpClient<IExchangeRateProvider, CnbApiExchangeRateProvider>();

        ServiceProvider sp = services.BuildServiceProvider();
        ILogger logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("Main");

        try
        {
            IExchangeRateProvider provider = sp.GetRequiredService<IExchangeRateProvider>();
            IReadOnlyList<ExchangeRate> rates = await provider.GetAsync(date, cts.Token);

            string effective = rates.First().ValidFor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.WriteLine($"CNB Exchange Rates (ValidFor={effective}) — {rates.Count} items:");
            foreach (ExchangeRate r in rates.OrderBy(r => r.SourceCurrency))
            {
                Console.WriteLine(r.ToString());
            }

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
}
