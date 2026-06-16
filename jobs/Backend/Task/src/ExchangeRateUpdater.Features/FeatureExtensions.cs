using Mews.Caching;
using System.Net;
using ExchangeRateUpdater.ApiClient.Client;
using ExchangeRateUpdater.Features.Configuration;
using ExchangeRateUpdater.Features.Services;
using ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using ExchangeRateUpdater.Features.Common;

namespace ExchangeRateUpdater.Features
{
    public static class FeatureExtensions
    {
        private static readonly int DEFAULT_WAIT_TIME_RETRY = 2;

        private static readonly string DEFAULT_TIMEOUT = "00:00:20";
        public delegate IAsyncPolicy<HttpResponseMessage> RetryPolicyFactory();

        public static void AddExchangeRateUpdaterFeature(
                    this IServiceCollection services,
                    Action<ExchangeRateFeatureConfiguration> configureOptions)
        {
            var options = new ExchangeRateFeatureConfiguration();
            configureOptions?.Invoke(options);

            if (options == null || string.IsNullOrEmpty(options.BaseUrl))
                throw new ArgumentNullException("Can not configure Exchange Rate updater feature if the cnb api url is null");

            services.AddLogging();
            services.AddCache();
            services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddScoped<IExchangeRateService, ExchangeRateService>();
            services.AddConfigureHttpClientPolicies(options);
        }


        private static void AddCache(this IServiceCollection services)
        {
            services.AddCustomCache(Constants.CacheName, opts =>
            {
                opts.Name = Constants.CacheName;
                opts.DefaultAbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            });
        }

        private static void AddConfigureHttpClientPolicies(this IServiceCollection services, ExchangeRateFeatureConfiguration options)
        {
            RetryPolicyFactory policyFactory = DefaultRetryPolicy;

            if (options.RetryOptions == RetryOptions.Custom && options.RetryHandler != null)
                policyFactory = () => options.RetryHandler();

            services.AddHttpClient<ICnbClient, CnbClient>(opts =>
            {
                opts.Timeout = options.Timeout ?? TimeSpan.Parse(DEFAULT_TIMEOUT);
                opts.BaseAddress = new Uri(options.BaseUrl);
            }).AddPolicyHandler(policyFactory());
        }


        private static IAsyncPolicy<HttpResponseMessage> DefaultRetryPolicy()
        {
            return Policy.HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.GatewayTimeout)
                .WaitAndRetryAsync(DEFAULT_WAIT_TIME_RETRY, retryAttempt => TimeSpan.FromSeconds(Math.Pow(DEFAULT_WAIT_TIME_RETRY, retryAttempt)));
        }
    }
}
