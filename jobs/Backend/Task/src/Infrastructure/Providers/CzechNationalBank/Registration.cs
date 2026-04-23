using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Configuration;
using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Clients;
using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Parsers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank;


public static class Registration
{
    public static IServiceCollection UseCzechNationalBankProvider(
        this IServiceCollection services,
        HostBuilderContext context)
    {
        services
            .AddOptions<ProviderOptions>()
            .Bind(context.Configuration.GetSection(ProviderOptions.ConfigurationSectionName))
            .Validate(opts =>
                {
                    opts.Validate();
                    return true;
                }
            );

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<ProviderOptions>>().Value);

        services
            .AddSingleton<IDailyExchangeRatesResponseParser, PipeSeparatedDailyExchangeResponseParser>()
            .AddScoped<Domain.Interfaces.IExchangeRateProvider, ExchangeRateProvider>();

        services
            .AddSingleton<PollyPolicies>()
            .AddHttpClient<ICzechNationalBankClient, CzechNationalBankHttpClient>()
            .AddPolicyHandler((sp, _) => sp.GetRequiredService<PollyPolicies>().RetryPolicy)
            .AddPolicyHandler((sp, _) => sp.GetRequiredService<PollyPolicies>().CircuitBreakerPolicy)
            .AddPolicyHandler((sp, _) => sp.GetRequiredService<PollyPolicies>().TimeoutPolicy);

        return services;
    }
}