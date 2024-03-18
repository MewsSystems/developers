namespace ExchangeRateUpdater.Console;

public static class Program
{
    private static readonly IEnumerable<Currency> currencies =
    [
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    ];

    public static async Task Main(string[] args)
    {
        var builder = CreateHostBuilder(args).Build();
        await builder.StartAsync();

        try
        {
            var provider = builder.Services.GetRequiredService<ExchangeRateProvider>();
            var rates = await provider.GetExchangeRates(currencies);

            System.Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
            foreach (var rate in rates)
            {
                System.Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        System.Console.ReadLine();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var redisUrl = Environment.GetEnvironmentVariable("REDIS_URL") ?? "localhost:6379";
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddJsonFile("appsettings.json").AddEnvironmentVariables();
            })
            .ConfigureServices(
                (hostContext, services) =>
                {
                    services.AddCNBClient();
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.ConfigurationOptions = new ConfigurationOptions
                        {
                            AbortOnConnectFail = false,
                            EndPoints = new() { redisUrl }
                        };
                        options.InstanceName = "ExchangeRatesUpdater";
                    });
                    services.Configure<RedisSettings>(x =>
                        x.ExpirationThreshold = TimeSpan.Parse(
                            hostContext.Configuration["Redis:ExpirationThreshold"]!
                        )
                    );
                    services.AddScoped<ICache, RedisCacheService>();
                    services.AddScoped<ExchangeRateProvider>();
                }
            );
    }
}
