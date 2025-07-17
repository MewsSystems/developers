using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater;

public static class HostHelper
{
    public static ServiceProvider CreateServiceProvider()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        
        var services = new ServiceCollection();
        services.AddOptions<ExchangeRateProviderSettings>().Bind(configuration.GetSection(nameof(ExchangeRateProviderSettings)));
        services.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>();
        return services.BuildServiceProvider();
    }
}