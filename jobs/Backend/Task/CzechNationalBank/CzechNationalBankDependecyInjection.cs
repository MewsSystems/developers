using ExchangeRateUpdater.Infrastructure.CzechNationalBank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Refit;

namespace ExchangeRateUpdater.CzechNationalBank
{
    internal static class CzechNationalBankDependecyInjection
    {
        internal static string ConfigurationSectionName = "CzechNationalBank:ExchangeRateAPI";

        internal static IServiceCollection AddCzechNationalBankClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<CzechNationalBankClientConfiguration>()
                    .Bind(configuration.GetSection(ConfigurationSectionName))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

            IEnumerable<TimeSpan> backoff = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(0.5), retryCount: 2);

            services.AddRefitClient<ICzechNationalBankClient>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var clientConfig = serviceProvider.GetRequiredService<IOptions<CzechNationalBankClientConfiguration>>().Value;

                    client.BaseAddress = new(clientConfig.BaseUrl);
                })
                .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(backoff));

            return services;
        }
    }
}
