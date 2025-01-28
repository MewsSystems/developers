using System;
using ExchangeRateUpdater.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PublicHoliday;

namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection WithCzechNationalBankRateSource(this IServiceCollection services, bool useDefaultUrls = true)
    {
        if (useDefaultUrls)
        {
            services.AddOptions<CzechNationalBankSourceOptions>().Configure(o =>
            {
                o.MainDataSourceUrl = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
                o.SecondaryDataSourceUrl = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt");
            });
        }
        
        services.TryAddSingleton<ICzechNationalBankRateParser, CzechNationalBankRateParser>();
        services.TryAddSingleton<ICzechNationalBankRateUriBuilder, CzechNationalBankRateUriBuilder>();
        services.TryAddSingleton<IExchangeRateCache, NullCache>();
        services.TryAddSingleton<CzechRepublicPublicHoliday>((_) => new CzechRepublicPublicHoliday() { UseCachingHolidays = true });
        services.TryAddSingleton<ICzechNationalBankRatesCacheExpirationCalculator, CzechNationalBankRatesCacheExpirationCalculator>();
        services.TryAddSingleton<TimeProvider>(TimeProvider.System);
        services.AddTransient<IRateSource, CzechNationalBankRateSource>();

        services.AddHttpClient<CzechNationalBankRateSource>(opts =>
        {
            opts.Timeout = TimeSpan.FromSeconds(30);
        });
        services.AddLogging();

        return services;
    }
}