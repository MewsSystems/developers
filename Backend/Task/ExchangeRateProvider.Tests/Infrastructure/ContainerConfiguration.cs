using ExchangeRateProvider.RateProviders;
using Unity;

namespace ExchangeRateProvider.Tests.Infrastructure
{
    public class ContainerConfiguration
    {
        private static UnityContainer _container;
        public static IUnityContainer Container { get => _container; }

        static ContainerConfiguration()
        {
            _container = new UnityContainer();

            _container.RegisterType<IRateProvider, CZKProvider>();
        }
    }
}
