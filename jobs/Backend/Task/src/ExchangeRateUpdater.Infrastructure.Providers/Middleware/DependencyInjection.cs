using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.Providers.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddThirdPartyProviders(this IServiceCollection services, IConfiguration? configuration = null)
    {
        // Todo Andrei: Review different injection scopes
        services.AddTransient<RefitLoggingHandler>();
        
        // Register Refit API clients first
        services.AddRefitClient<ICzechNationalBankApiClient>()
            .ConfigureHttpClient(c =>
            {
                // Use configuration if available, otherwise use defaults
                var baseUrl = configuration?["CzechNationalBank:BaseUrl"] ?? "https://api.cnb.cz/cnbapi/";
                var timeout = configuration?.GetValue<int>("CzechNationalBank:TimeoutSeconds") ?? 30;
                
                c.BaseAddress = new Uri(baseUrl);
                c.Timeout = TimeSpan.FromSeconds(timeout);
            })
            .AddHttpMessageHandler<RefitLoggingHandler>();

        // Register exchange rate providers
        // Note: Using AddSingleton with factory pattern to provide the providerName parameter
        services.AddSingleton<IExchangeRateProvider>(serviceProvider =>
        {
            var cnbApiClient = serviceProvider.GetRequiredService<ICzechNationalBankApiClient>();
            return new CnbExchangeRateProvider(cnbApiClient, "CzechNationalBank");
        });

        // Example: If you want to add more providers in the future, you can add them here:
        // services.AddSingleton<IExchangeRateProvider>(serviceProvider =>
        // {
        //     var otherApiClient = serviceProvider.GetRequiredService<IOtherApiClient>();
        //     return new OtherExchangeRateProvider(otherApiClient, "OtherProvider");
        // });
         
        return services;
    }
} 