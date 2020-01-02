using Unity;
using ExchangeRateProvider.Infrastructure;
using ExchangeRateProvider;
using ExchangeRateProvider.RateProviders;

namespace ExchangeRateUpdater.Infrastructure
{
    public class ContainerConfiguration
    {
        private static UnityContainer _container;
        public static IUnityContainer Container { get => _container; }

        static ContainerConfiguration()
        {
            _container = new UnityContainer();

            _container.RegisterType<IRateProviderSettings, RateProviderSettings>();
            _container.RegisterType<IRateProvider, CZKProvider>();
        }
    }
}
