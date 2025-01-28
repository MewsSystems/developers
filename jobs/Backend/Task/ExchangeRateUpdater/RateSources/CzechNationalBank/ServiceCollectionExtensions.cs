using System;
using ExchangeRateUpdater.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PublicHoliday;

namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCzechNationalBankRateSourceWithAllDependencies(this IServiceCollection services, Action<CzechNationalBankSourceOptions> configureSourceOptions)
    {
        services.Configure(configureSourceOptions);
        services.TryAddSingleton<ICzechNationalBankRateParser, CzechNationalBankRateParser>();
        services.TryAddSingleton<ICzechNationalBankRateUriBuilder, CzechNationalBankRateUriBuilder>();
        services.TryAddSingleton<IExchangeRateCache, NullCache>();
        services.TryAddSingleton<CzechRepublicPublicHoliday>((_) => new CzechRepublicPublicHoliday() { UseCachingHolidays = true });
        services.TryAddSingleton<ICzechNationalBankRatesCacheExpirationCalculator, CzechNationalBankRatesCacheExpirationCalculator>();
        services.AddTransient<IRateSource, CzechNationalBankRateSource>();

        services.AddHttpClient<CzechNationalBankRateSource>(opts =>
        {
            opts.Timeout = TimeSpan.FromSeconds(30);
        });
        services.AddLogging();

        return services;
    }
}