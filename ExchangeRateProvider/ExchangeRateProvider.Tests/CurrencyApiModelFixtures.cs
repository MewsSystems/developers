using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit;
using NUnit.Engine;
using FluentAssertions.Common;
using FluentAssertions;
using System.Net.Http;
using Moq;
using Moq.Protected;
using System.Threading;
using System.Net;
using ExchangeRateProvider.Infrastructure.ApiProxy;
using ExchangeRateProvider.Infrastructure.HttpHelper;
using ExchangeRateProvider.Model;
using ExchangeRateProvider.Model.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

using ExchangeRateProvider.Tests.Properties;
using StructureMap;
using StructureMap.Graph;

namespace ExchangeRateProvider.Tests
{

    [TestFixture]
    public class CurrencyApiModelFixtures
    {
        /// <summary>
        /// Container
        /// </summary>
        /// <returns></returns>
        public static Container Container()
        {
            var container = new Container();
            container.Configure(expr =>
            {
                expr.For<IHttpHelper>().Use(x => new HttpHelperAsync()).ContainerScoped();
                expr.For<ApiProxy>().Use<ServiceLocatorFixture.CurrencyApiProxy>()
                    .Ctor<ServiceLocatorFixture.CurrencyApiProxy>(MessagingInfrastructureFixtures.Config.CurrenciesApiUrl)
                    .IsTheDefault()
                    .ContainerScoped();
                expr.Scan(scanner => scanner.AssembliesAndExecutablesFromApplicationBaseDirectory());
            });
            return container;
        }


        /// <summary>
        /// CurrencyApiResponseDeserializeTableEntries
        /// </summary>
        /// <exception cref="HttpRequestException"></exception>
        [Test]
        public void CurrencyApiResponseDeserializeTableEntries ()
        {
            using (var container = Container())
            {
                Assert.That(container, Is.Not.Null);
                var helper = container?.GetInstance<IHttpHelper>();
                Assert.That(helper, Is.Not.Null);

                Assert.DoesNotThrow(() =>
                {
                    helper?.Get<string>( MessagingInfrastructureFixtures.Config.CurrenciesApiUrl,
                        s => Assert.That(JsonConvert.DeserializeObject(s)?.As<RootObject>()?.TableEntries, Is.Not.Null),
                        err => { throw new HttpRequestException(err?.ExceptionMessage); });
                });

            }
        }

        [Test]
        public void CurrencyApiTableEntriesEnumerateToCurrencyDto()
        {
            using (var container = Container())
            {
                Assert.That(container, Is.Not.Null);
                var helper = container?.GetInstance<IHttpHelper>();
                Assert.That(helper, Is.Not.Null);

                    helper?.Get<string>(MessagingInfrastructureFixtures.Config.CurrenciesApiUrl,
                        s =>
                        {
                            var rates = JsonConvert.DeserializeObject(s)?.As<RootObject>();
                            var currencyList = rates?.TableEntries?.Select(tableEntry =>
                                new CurrencyDto {
                                    Code = tableEntry?.Id
                                });

                            Assert.That(currencyList, Is.Empty);
                        },
                        err =>
                        {
                            throw new HttpRequestException(err?.ExceptionMessage);
                        });
            }
        }


        [Test]
        public void CurrencyApiTableEntriesEnumerateToExchangeRates()
        {
            using (var container = Container())
            {
                Assert.That(container, Is.Not.Null);
                var helper = container?.GetInstance<IHttpHelper>();
                Assert.That(helper, Is.Not.Null);

                helper?.Get<string>(MessagingInfrastructureFixtures.Config.CurrenciesApiUrl,
                    s =>
                    {
                        var rates = JsonConvert.DeserializeObject(s)?.As<RootObject>();
                        var currencyList = rates?.TableEntries?.AsExchangeRateEnumerable();
                        Assert.That(currencyList, Is.Empty);
                    },
                    err =>
                    {
                        throw new HttpRequestException(err?.ExceptionMessage);
                    });
            }
        }
    }

}