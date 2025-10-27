using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Jobs;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Interfaces;
using ExchangeRateUpdater.Services.Models.External;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Quartz;

namespace ExchangeRateUpdater.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfiguration = configuration
                .GetSection(Constants.ApiConfiguration)
                .Get<ApiConfiguration>();

            var httpClientBuilder = services.AddHttpClient<CnbApiClient>(
                apiConfiguration!.Name,
                o =>
                {
                    o.BaseAddress = new Uri(apiConfiguration.BaseUrl);
                    o.Timeout = TimeSpan.FromSeconds(apiConfiguration.RequestTimeoutInSeconds);

                    if (apiConfiguration.DefaultRequestHeaders.Count == 0)
                        return;

                    foreach (var entry in apiConfiguration.DefaultRequestHeaders)
                    {
                        o.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                    }
                })

                .AddPolicyHandler((serviceProvider, _) =>
                    CreateDefaultTransientRetryPolicy(serviceProvider, apiConfiguration.RetryTimeOutInSeconds));

            services.AddQuartz(q =>
             {
                 var jobKey = new JobKey("ExchangeRateRefreshJob");

                 q.AddJob<ExchangeRateRefreshJob>(opts => opts.WithIdentity(jobKey));

                 // Scheduled trigger (14:30:30 CEST weekdays), allowing a buffer of 30 seconds
                 q.AddTrigger(opts => opts
                     .ForJob(jobKey)
                     .WithIdentity("ExchangeRateRefreshTrigger")
                     .WithSchedule(CronScheduleBuilder
                         .CronSchedule("30 30 14 ? * MON-FRI")
                         .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"))));

                 // Immediate trigger on startup
                 q.AddTrigger(opts => opts
                     .ForJob(jobKey)
                     .WithIdentity("ExchangeRateStartupTrigger")
                     .StartNow());
             });

            services.AddMemoryCache();
            services.AddQuartzHostedService();
            services.AddSingleton(apiConfiguration);
            services.AddScoped<IDateTimeSource, DateTimeSource>();
            services.AddScoped<IExchangeRateCacheService, ExchangeRateCacheService>();
            httpClientBuilder.AddTypedClient<IApiClient<CnbRate>, CnbApiClient>();
            services.AddScoped<ExchangeRateProvider>();
        }

        // Set up up to 3 retries with Polly with random jitter
        private static IAsyncPolicy<HttpResponseMessage> CreateDefaultTransientRetryPolicy(
            IServiceProvider provider,
            int retryTimeOut)
        {
            var jitter = new Random();
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .OrResult(
                    msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .OrResult(msg => msg?.Headers.RetryAfter != null)
                .WaitAndRetryAsync(
                    3,
                    (retryCount, response, _) =>
                        response.Result?.Headers.RetryAfter?.Delta ??
                        TimeSpan.FromSeconds(
                            Math.Pow(2, retryCount))
                        + TimeSpan.FromMilliseconds(
                            jitter.Next(0, 100)),
                    (result, span, count, _) =>
                    {
                        LogRetry(provider, result, span, count);
                        return Task.CompletedTask;
                    });

            return Policy.WrapAsync(retryPolicy, Policy.TimeoutAsync<HttpResponseMessage>(retryTimeOut));
        }

        // Log retry attempts
        private static void LogRetry(
            IServiceProvider provider,
            DelegateResult<HttpResponseMessage> response,
            TimeSpan span,
            int retryCount)
        {
            var logger = provider.GetService<ILogger<CnbApiClient>>();
            if (logger == null)
            {
                throw new NullReferenceException("Null reference for logger during retry");
            }

            var responseMsg = response.Result;
            if (responseMsg is null)
            {
                logger.LogError(
                    response.Exception,
                    "Retry attempt [{RetryCount}] with delay of [{Time}]ms",
                    retryCount,
                    span.TotalMilliseconds);
            }
            else
            {
                logger.LogWarning(
                    "Retry attempt [{RetryCount}] with delay of [{Time}]ms [{@Response}]",
                    retryCount,
                    span.TotalMilliseconds,
                    new
                    {
                        responseMsg.StatusCode,
                        responseMsg.Content,
                        responseMsg.Headers,
                        responseMsg.RequestMessage?.RequestUri
                    });
            }
        }
    }
}
