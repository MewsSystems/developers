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
using Newtonsoft.Json;
using NUnit.Framework;

using ExchangeRateProvider.Tests.Properties;
using StructureMap;
using StructureMap.Graph;

namespace ExchangeRateProvider.Tests
{
    [Serializable]
    [JsonObject]
    public class TableEntry
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int ConversionFactor { get; set; }

        public List<decimal> Values { get; set; }
        public string GraphUrl { get; set; }
    }

    [Serializable]
    [JsonObject]
    public class RootObject
    {
        public string Updated { get; set; }
        public string TableNameHeader { get; set; }
        public string TableGraphHeader { get; set; }
        public List<string> TableDynamicHeaders { get; set; }
        public List<TableEntry> TableEntries { get; set; }
    }

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
                expr.For<IHttpHelper>().Use(x => new HttpHelper()).ContainerScoped();
                expr.For<ApiProxy>().Use<ServiceLocatorFixture.CurrencyApiProxy>().Ctor<ServiceLocatorFixture.CurrencyApiProxy>("Currency").IsTheDefault().ContainerScoped();
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

                Assert.DoesNotThrow(() =>
                {
                    helper?.Get<string>(MessagingInfrastructureFixtures.Config.CurrenciesApiUrl,
                        s =>
                        {
                            var rates = JsonConvert.DeserializeObject(s)?.As<RootObject>();
                            var currencyList = rates?.TableEntries?.Select(tableEntry =>
                                new CurrencyDto {
                                    Code = tableEntry?.Id
                                });

                            Assert.That(currencyList, Is.Not.Empty);
                        },
                        err => { throw new HttpRequestException(err?.ExceptionMessage); });
                });

            }
        }
    }

}