using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace ExchangeRateUpdater;

public static class Program
{
    public static int Main(string[] args)
    {
        using IHost host = BuildHost(args);
        ILogger logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("ExchangeRateUpdater");

        try
        {
            IExchangeRateProvider provider = host.Services.GetRequiredService<IExchangeRateProvider>();
            IEnumerable<Currency> currencies = BuildRequestedCurrencies(args);
            IEnumerable<ExchangeRate> rates = provider.GetExchangeRates(currencies);

            foreach (ExchangeRate rate in rates)
            {
                logger.LogInformation("{Rate}", rate);
            }

            return 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve exchange rates from CNB.");
            return 1;
        }
    }

    internal static IHost BuildHost(string[] args)
    {
        HostApplicationBuilderSettings settings = new()
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory
        };
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(settings);

        builder.Services
            .AddOptions<CnbOptions>()
            .Bind(builder.Configuration.GetSection(CnbOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddSingleton<IValidateOptions<CnbOptions>, CnbOptionsValidator>();
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
        builder.Services.AddHttpClient("cnb")
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                CnbOptions options = serviceProvider.GetRequiredService<IOptions<CnbOptions>>().Value;
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            })
            .AddResilienceHandler(
                "cnb-pipeline",
                static (pipelineBuilder, context) =>
                {
                    CnbOptions options = context.ServiceProvider.GetRequiredService<IOptions<CnbOptions>>().Value;
                    pipelineBuilder.AddRetry(new HttpRetryStrategyOptions
                    {
                        MaxRetryAttempts = options.MaxRetries,
                        BackoffType = DelayBackoffType.Exponential,
                        UseJitter = true,
                        ShouldHandle = static args =>
                        {
                            int statusCode = args.Outcome.Result is null ? 0 : (int)args.Outcome.Result.StatusCode;
                            bool shouldRetryStatus = statusCode == 408 || statusCode == 429 || statusCode >= 500;
                            bool shouldRetryException = args.Outcome.Exception is HttpRequestException or OperationCanceledException;
                            return ValueTask.FromResult(shouldRetryStatus || shouldRetryException);
                        }
                    });
                });

        return builder.Build();
    }

    private static IEnumerable<Currency> BuildRequestedCurrencies(string[] args)
    {
        if (args.Length > 0)
        {
            foreach (string arg in args)
            {
                yield return new Currency(arg);
            }

            yield break;
        }

        yield return new Currency("EUR");
        yield return new Currency("USD");
    }
}
