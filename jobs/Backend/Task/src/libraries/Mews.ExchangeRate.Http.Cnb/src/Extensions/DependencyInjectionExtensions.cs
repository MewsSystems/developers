using Mews.ExchangeRate.Http.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Mews.ExchangeRate.Http.Cnb.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddExchangeRateHttpServices(
            this IServiceCollection services
            )
        {
            // Configure HTTP client with default retry policies:
            services
                .AddHttpClient<IHttpClient, ExchangeRateServiceHttpClient>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

            services
                .AddScoped<IExchangeRateServiceClient, ExchangeRateServiceClient>();

            return services;
        }

        /// <summary>
        /// Gets the retry policy.
        /// </summary>
        /// <returns></returns>
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}