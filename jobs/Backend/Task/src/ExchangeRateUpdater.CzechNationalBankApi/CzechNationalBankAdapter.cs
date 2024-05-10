using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.CzechNationalBank.Api;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net;

namespace ExchangeRateUpdater.CzechNationalBank
{
    public static class CzechNationalBankAdapter
    {
        internal static readonly string ApiClientName = "CzechNationalBank.Api";

        public static IServiceCollection ConfigureCzechNationalBank(this IServiceCollection service, CzechNationalBankConfiguration configuration)
        {
            service.AddSingleton(configuration);
            service.AddSingleton<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
            service.AddSingleton<ICzechNationalBankApi, CzechNationalBankApi>();

            service.AddHttpClient(ApiClientName, x =>
                {
                    x.BaseAddress = configuration.ApiBaseUrl;
                }).AddPolicyHandler(Policy
                    .Handle<HttpRequestException>() 
                    .OrResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));

            return service;
        }
    }
}
