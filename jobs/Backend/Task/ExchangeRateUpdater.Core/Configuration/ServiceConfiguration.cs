using ExchangeRateUpdater.Core.Configuration.Options;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Core.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
        services.AddOptions<CurrencyOptions>().Configure(opts =>
            {
                var currencies = configuration.GetSection("Currencies").Get<string[]>() ?? [];
                opts.Currencies = currencies.Select(s => new Currency(s)).ToArray();
            })
            .ValidateOnStart();
        
        return services;
    }
}