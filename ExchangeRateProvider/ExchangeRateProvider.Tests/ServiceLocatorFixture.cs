using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExchangeRateProvider.Infrastructure.ApiProxy;
using ExchangeRateProvider.Infrastructure.HttpHelper;

using CommonServiceLocator.StructureMapAdapter.Unofficial;

using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using StructureMap;
using StructureMap.Graph;

namespace ExchangeRateProvider.Tests
{
    [TestFixture]
    public class ServiceLocatorFixture
    {
        public static Container Container()
        {
            var container = new Container();
            container.Configure(expr =>
            {
                expr.For<IHttpHelper>().Use(x => new HttpHelper()).ContainerScoped();
                expr.For<ApiProxy>().Use<CurrencyApiProxy>().Ctor<CurrencyApiProxy>("Currency").IsTheDefault().ContainerScoped();
                expr.Scan((scanner) => scanner.AssembliesAndExecutablesFromApplicationBaseDirectory());
            });
            return container;
        }

        /// <summary>
        /// test implementation for currency api proxy instance
        /// </summary>
        public sealed class CurrencyApiProxy :ApiProxy
        {
            public CurrencyApiProxy(string controllerName = "/api/Currency") : base(controllerName)
            {
            }
        }

        [Test]
        public void ServiceLocatorProviderInstanceShouldBeCreated()
        {
            using (var container = Container())
            {
                ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));
                Assert.That(ServiceLocator.Current, Is.Not.Null);
            }
        }

        [Test]
        public void ServiceLocatorProviderInstanceShouldBeCreatedWithEmptyCompositionRoot()
        {
            using (var container = new StructureMap.Container())
            {
                ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));
                Assert.That(ServiceLocator.Current, Is.Not.Null);
            }
        }

        [Test]
        public void ServiceLocatorShouldReturnHttpHelperInstance()
        {
            using (var container = Container())
            {
                ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));

                Assert.That(ServiceLocator.Current, Is.Not.Null);
                Assert.That(ServiceLocator.Current.GetInstance(typeof (IHttpHelper)), Is.Not.Null);
            }
        }

        [Test]
        public void ServiceLocatorShouldReturnApiProxyInstance()
        {
            using (var container = Container())
            {
                ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));

                Assert.That(ServiceLocator.Current, Is.Not.Null);
                Assert.That(ServiceLocator.Current.GetInstance(typeof(ApiProxy)), Is.Not.Null);
            }
        }
    }
}
