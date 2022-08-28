using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Tests;

public class TestBase
{
    protected IExchangeRateProvider _exchangeRateProvider;

    public TestBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: false)
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IExchangeRateProvider, CNBRateProviderService>() //Note: can be swapped with BOERateProviderService if required...
            .Configure<AppConfig>(configuration.GetSection(AppConfig.SectionKey))
            .AddHttpClient()
            .AddLogging(configure => configure.AddConsole())
            .BuildServiceProvider();

        //assign services for testing
        _exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
    }
}