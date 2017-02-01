using System.Threading.Tasks;
using CommonServiceLocator.StructureMapAdapter.Unofficial;
using ExchangeRateProvider.Infrastructure.ApiProxy;
using ExchangeRateProvider.Infrastructure.HttpHelper;
using FluentAssertions;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using StructureMap;
using StructureMap.Graph;

namespace ExchangeRateProvider.Tests
{
    [TestFixture]
    public class CurrencyApiProxyFixtures
    {
        private static Container Container()
        {

                var container = new Container();
            container.Configure(expr =>
            {

                expr.For<IHttpHelper>().Use(x => new HttpHelperAsync()).ContainerScoped();
                expr.For<ApiProxy>().Use<CurrencyApiProxy>()
                    .Ctor<CurrencyApiProxy>()
                    .IsTheDefault()
                    .ContainerScoped();
                expr.Scan(scanner => scanner.AssembliesAndExecutablesFromApplicationBaseDirectory());
            });

            return container;
        }

        [Test]
        public async Task CurrencyApiProxyShouldGetExchangeRates()
        {
            using (var container = Container())
            {
                ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));
                var exchangeRateDtos = await ServiceLocator.Current.GetInstance<ApiProxy>()
                    .As<CurrencyApiProxy>()
                    .GetExchangeRatesAsync();

                Assert.That(exchangeRateDtos, Is.Null);
            }
        }

    }
}