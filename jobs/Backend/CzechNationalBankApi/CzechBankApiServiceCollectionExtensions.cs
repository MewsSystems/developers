using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;

namespace CzechNationalBankApi
{
    public static class CzechBankApiServiceCollectionExtensions
    {
        public static void AddCzechBankApiService(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfiguration = configuration.GetSection(nameof(CzechBankApiServiceOptions)).Get<CzechBankApiServiceOptions>() ?? 
                    throw new ArgumentNullException(nameof(CzechBankApiServiceOptions), $"Could not load required appsettings for {nameof(CzechBankApiServiceOptions)}");

            services.AddSingleton(apiConfiguration);

            services.AddHttpClient<ICzechBankApiService, CzechBankApiService>(options =>
            {
                options.BaseAddress = new Uri(apiConfiguration.BaseUrl);

            }).AddPolicyHandler(GetTransientRetryPolicy());
        }

        private static IAsyncPolicy<HttpResponseMessage> GetTransientRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}
