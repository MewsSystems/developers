using System;
using System.Collections.Generic;
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
using NUnit.Framework;

namespace ExchangeRateProvider.Tests
{
    /// <summary>
    /// Core messaging test fixture
    /// </summary>
    [TestFixture]
    public class MessagingInfrastructureFixtures
    {
        /// <summary>
        /// Config
        /// </summary>
        internal static class Config
        {
            public const string CurrenciesApiUrl = @"http://www.norges-bank.no/api/Currencies?frequency=D2&language=en&idfilter=none&observationlimit=2&returnsdmx=false";
        }

        /// <summary>
        /// Should get rates over http transport via provided url
        /// </summary>
        /// <param name="ratesUrl">
        ///
        /// </param>
        [Test]
        [TestCase(Config.CurrenciesApiUrl)]

        [TestCase("http://www.norges-bank.no/api/Currencies")]
        public async Task ShouldGetRatesOverHTTPTransportAsync(string ratesUrl = Config.CurrenciesApiUrl)
        {
            Assert.That(ratesUrl, Is.Not.Null.Or.Empty);

            var handler = new Mock<HttpMessageHandler>();

            handler.Protected()
                   .Setup<Task<HttpResponseMessage>>(@"SendAsync",ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .Returns(Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK)))
                   .Callback<HttpRequestMessage, CancellationToken>((r, c) => Assert.AreEqual(HttpMethod.Get, r.Method));

            using (var client = new HttpClient(handler.Object))
            {
                Assert.That(client, Is.Not.Null);

                var request = new HttpRequestMessage(HttpMethod.Get, ratesUrl);
                var response = await client.SendAsync(request).ConfigureAwait(false);

                Assert.That(response, Is.Not.Null);
                Assert.That(response.IsSuccessStatusCode, Is.True);
            }
        }

    }
}
