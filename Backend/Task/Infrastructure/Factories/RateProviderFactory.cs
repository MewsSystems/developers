using ExchangeRateProvider;
using Unity;

namespace ExchangeRateUpdater.Infrastructure.Factories
{
    public class RateProviderFactory
    {
        public static IRateProvider Create()
        {
            return ContainerConfiguration.Container.Resolve<IRateProvider>();
        }
    }
}
