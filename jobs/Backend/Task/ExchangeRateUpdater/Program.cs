using System.Net;
using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Polly;
using Polly.Retry;

namespace ExchangeRateUpdater;

public static class Program
{
    private static readonly CancellationTokenSource Cts = new();

    private static readonly Currency[] Currencies = new[] {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

    public static async Task Main(string[] args)
    {
        Console.CancelKeyPress += (_, _) => Cts.Cancel();

        using var openTelemetry = BuildOTel();
        var loggerFactory = BuildLoggerFactory();

        try
        {
            using var httpClient = BuildHttpClient(TimeSpan.FromSeconds(8));
            using var provider = BuildExchangeRateProvider(httpClient, loggerFactory);

            var ratesResult = await provider.GetExchangeRates(Currencies, Cts.Token);
            ratesResult.Switch(
                rates =>
                {
                    Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
                    foreach (var rate in rates)
                    {
                        Console.WriteLine(rate.ToString());
                    }
                },
                error => Console.WriteLine(error.Message));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }

        Console.ReadLine();
    }

    private static TracerProvider BuildOTel() =>
        Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation()
            .SetSampler(new AlwaysOnSampler())
            .AddConsoleExporter()
            .Build();

    private static ILoggerFactory BuildLoggerFactory() =>
        LoggerFactory.Create(
            builder => builder
                .SetMinimumLevel(LogLevel.Debug)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddConsole());

    private static HttpClient BuildHttpClient(TimeSpan requestTimeout)
    {
        var resiliencePipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(
                new RetryStrategyOptions<HttpResponseMessage>
                {
                    BackoffType = DelayBackoffType.Exponential,
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(2),
                    UseJitter = true,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .HandleResult(
                            response =>
                                response.StatusCode == HttpStatusCode.RequestTimeout
                                || response.StatusCode == HttpStatusCode.TooManyRequests
                                || response.StatusCode >= HttpStatusCode.InternalServerError),
                })
            .AddConcurrencyLimiter(32)
            .Build();

        var policyHttpMessageHandler = new PolicyHttpMessageHandler(resiliencePipeline.AsAsyncPolicy())
        {
            InnerHandler = new HttpClientHandler()
        };

        return new HttpClient(policyHttpMessageHandler)
        {
            Timeout = requestTimeout
        };
    }

    private static ExchangeRateProvider BuildExchangeRateProvider(HttpClient httpClient, ILoggerFactory loggerFactory)
    {
        // 💡 in application using host builder, all this would be configured and resolved by framework
        var options = Options.Create(
            new ExchangeRateProviderOptions
            {
                CacheTtl = TimeSpan.FromMinutes(8)
            });

        var cnbClient = new CnbClient(httpClient, loggerFactory.CreateLogger<CnbClient>());

        return new ExchangeRateProvider(options, cnbClient, loggerFactory.CreateLogger<ExchangeRateProvider>());
    }
}