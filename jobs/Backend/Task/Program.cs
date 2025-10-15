using System;
using System.Globalization;
using System.Linq;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(cfg =>
    {
        cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddOptions<ExchangeRateSettings>()
                .Bind(configuration.GetSection("ExchangeRateSettings"))
                .ValidateDataAnnotations()
                .Validate(s => s.CnbDailyUrl is not null && s.CnbDailyUrl.IsAbsoluteUri, "CnbDailyUrl must be absolute")
                .Validate(s => s.CnbDailyUrl.Scheme == Uri.UriSchemeHttps, "CnbDailyUrl must be HTTPS")
                .ValidateOnStart();

        services.AddMemoryCache();

        services.AddHttpClient<ICnbClient, CnbClient>((sp, client) =>
        {
            var s = sp.GetRequiredService<IOptions<ExchangeRateSettings>>().Value;
            client.Timeout = TimeSpan.FromSeconds(s.HttpTimeoutSeconds);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("ExchangeRateUpdater/1.0");
            client.DefaultRequestHeaders.Accept.ParseAdd("text/plain");
            client.BaseAddress = s.CnbDailyUrl; 
        });

        services.AddSingleton<ICnbParser, CnbParser>();
        services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

using var scope = host.Services.CreateScope();
var sp = scope.ServiceProvider;
var logger = sp.GetRequiredService<ILogger<Program>>();

try
{
    var settings  = sp.GetRequiredService<IOptions<ExchangeRateSettings>>().Value;
    var client    = sp.GetRequiredService<ICnbClient>();
    var provider  = sp.GetRequiredService<IExchangeRateProvider>();

    // Fetch payload once to detect decimal style used by the TXT feed.
    var payload       = await client.GetDailyRatesAsync(default);
    var outputCulture = CnbParser.DetectCulture(payload); 

    // 2) currencies from config.
    var currencies = settings.Currencies.Select(c => new Currency(c.Trim())).ToArray();
    if (currencies.Length == 0)
    {
        logger.LogWarning("No currencies configured in appsettings.json.");
    }
    else
    {
        // Get per-unit rates and print with detected culture.
        var rates = await provider.GetExchangeRatesAsync(currencies, default);
        foreach (var r in rates)
        {
            var formatted = r.Value.ToString("0.######", outputCulture);
            Console.WriteLine($"{r.SourceCurrency}/{r.TargetCurrency}={formatted}");
        }
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred while fetching exchange rates.");
}
