using ExchangeRateUpdater.DataSource;
using ExchangeRateUpdater.Deserializers;
using ExchangeRateUpdater.Provider;
using Microsoft.Extensions.Configuration;
using Unity;
using Unity.Lifetime;

namespace ExchangeRateUpdater;

public static class UnityContainerFactory
{
    public static IUnityContainer Create(string appSettingsFile)
    {
        var container = new UnityContainer();

        container.RegisterInstance(typeof(string), ExchangeRateUpdaterRegistrationKeys.AppSettingsFile, appSettingsFile);
        container.RegisterFactory<IConfigurationRoot>(CreateConfigRoot, new ContainerControlledLifetimeManager());
        container.RegisterFactory<IExchangeRateDataSourceProvider>(CreateDataSourceProvider, new ContainerControlledLifetimeManager());
        container.RegisterType<IExchangeRateDeserializer, CzechNationalBankExchangeRateDeserializer>(new ContainerControlledLifetimeManager());
        container.RegisterType<IExchangeRatesDeserializer, CzechNationalBankExchangeRatesDeserializer>(new ContainerControlledLifetimeManager());
        container.RegisterType<IExchangeRateProvider, ExchangeRateProvider>();

        return container;
    }

    private static object CreateDataSourceProvider(IUnityContainer container)
    {
        var config = container.Resolve<IConfigurationRoot>();
        var url = config.GetSection(ExchangeRateUpdaterAppSettingsKeys.CNBExchangeRateUrl).Value;
        return new RestExchangeRateDataSourceProvider(url);
    }

    private static IConfigurationRoot CreateConfigRoot(IUnityContainer container)
    {
        var appSettingsFile = container.Resolve<string>(ExchangeRateUpdaterRegistrationKeys.AppSettingsFile);
        return new ConfigurationBuilder().AddJsonFile(appSettingsFile).Build();
    }
}