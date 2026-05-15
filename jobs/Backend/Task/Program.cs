using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Hosting;
using Polly;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        private static IEnumerable<Currency> currencies = new[]
        {
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

        public static void Main(string[] args)
        {
            try
            {
                var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
                {
                    Args = args,
                    ContentRootPath = AppContext.BaseDirectory,
                });

                builder.Services.Configure<CnbOptions>(
                    builder.Configuration.GetSection(CnbOptions.SectionName)); 

                var cnbOptions = builder.Configuration
                    .GetSection(CnbOptions.SectionName)
                    .Get<CnbOptions>() ?? new CnbOptions();
                ValidateCnbOptions(cnbOptions);

                builder.Services
                    .AddHttpClient<IExchangeRateSource, CnbExchangeRateSource>(client =>
                    {
                        client.Timeout = Timeout.InfiniteTimeSpan;
                    })
                    .AddStandardResilienceHandler(options =>
                    {
                        options.TotalRequestTimeout.Timeout =
                            TimeSpan.FromSeconds(cnbOptions.TotalTimeoutSeconds);
                        options.AttemptTimeout.Timeout =
                            TimeSpan.FromSeconds(cnbOptions.AttemptTimeoutSeconds);
                        options.Retry.MaxRetryAttempts = cnbOptions.RetryCount;
                        options.Retry.Delay =
                            TimeSpan.FromMilliseconds(cnbOptions.RetryDelayMilliseconds);
                        options.Retry.BackoffType = DelayBackoffType.Exponential;
                        options.Retry.UseJitter = true;
                    });

                builder.Services.AddTransient<ExchangeRateProvider>();
                using var host = builder.Build();
                var exchangeRateProvider = host.Services.GetRequiredService<ExchangeRateProvider>();
                var rates = exchangeRateProvider.GetExchangeRates(currencies);

                Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
                foreach (var rate in rates) Console.WriteLine(rate.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        private static void ValidateCnbOptions(CnbOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.DailyKurzUrl)
                || !Uri.TryCreate(options.DailyKurzUrl, UriKind.Absolute, out _))
            {
                throw new InvalidOperationException("Cnb:DailyKurzUrl must be configured as an absolute URL.");
            }

            if (options.TotalTimeoutSeconds <= 0)
            {
                throw new InvalidOperationException("Cnb:TotalTimeoutSeconds must be greater than zero.");
            }

            if (options.AttemptTimeoutSeconds <= 0)
            {
                throw new InvalidOperationException("Cnb:AttemptTimeoutSeconds must be greater than zero.");
            }

            if (options.AttemptTimeoutSeconds > options.TotalTimeoutSeconds)
            {
                throw new InvalidOperationException("Cnb:AttemptTimeoutSeconds must not exceed Cnb:TotalTimeoutSeconds.");
            }

            if (options.RetryCount < 0)
            {
                throw new InvalidOperationException("Cnb:RetryCount must not be negative.");
            }

            if (options.RetryDelayMilliseconds < 0)
            {
                throw new InvalidOperationException("Cnb:RetryDelayMilliseconds must not be negative.");
            }
        }
    }
}
