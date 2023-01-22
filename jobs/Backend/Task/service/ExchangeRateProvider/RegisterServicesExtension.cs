using ExchangeRateProviderCzechNationalBank.Interface;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System.Net;

namespace ExchangeRateProviderCzechNationalBank
{
    public static class RegisterServicesExtension
    {
        public static void RegisterExchangeRateProviderForCzechNationalBank(this IServiceCollection services)
        {
            services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

            services.AddSingleton<IStoreExchangeRates, StoreExchangeRates>();

            //It would be nice to set this HttpClient log level lower without appsettings
            services.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>(client =>
            {
                client.BaseAddress = new Uri("https://www.cnb.cz/cs/");
            }).AddTransientHttpErrorPolicy(policyBuilder => 
                policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 3)));

            //Removes all previous HttpClients logging - no spam, but DANGEROUS!
            //services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
        }
    }
}
