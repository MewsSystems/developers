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
using Newtonsoft.Json;
using NUnit.Framework;

using ExchangeRateProvider.Tests.Properties;

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
        /// ResourceProvider
        /// </summary>
        internal static class ResourceProvider
        {
            /// <summary>
            /// example exchange rates <c>json</c> response
            /// </summary>
            public static string ApiResponseJsonString {get;} = Resources.ApiResponseJsonString;
        }

        /// <summary>
        /// Should get rates over http transport via provided url
        /// </summary>
        /// <param name="ratesUrl">
        ///
        /// </param>
        [Test]
        [TestCase(Config.CurrenciesApiUrl)]
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


        /// <summary>
        /// Parse <c>json</c> string from HttpResponseMessage.Content
        /// </summary>
        /// <param name="ratesUrl">
        /// exchange rates api
        /// </param>
        [Test]
        [TestCase(Config.CurrenciesApiUrl)]
        [Description("Mock Currency Api Response")]
        public async Task ShouldGetJsonResponseOverHTTPTransportAsync(string ratesUrl)
        {
            Assert.That(ratesUrl, Is.Not.Null.Or.Empty);

            var handler = new Mock<HttpMessageHandler>();

            handler.Protected()
                   .Setup<Task<HttpResponseMessage>>(@"SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .Returns(Task<HttpResponseMessage>.Factory.StartNew(() =>
                        new HttpResponseMessage(HttpStatusCode.OK) {
                            Content = new StringContent(ResourceProvider.ApiResponseJsonString),
                        }))
                   .Callback<HttpRequestMessage, CancellationToken>((requestMessage, cancellationToken) =>
                       Assert.AreEqual(HttpMethod.Get, requestMessage.Method));

            using (var client = new HttpClient(handler.Object))
            {
                Assert.That(client, Is.Not.Null);


                var request = new HttpRequestMessage(HttpMethod.Get, ratesUrl);
                var response = await client.SendAsync(request).ConfigureAwait(false);

                Assert.That(response, Is.Not.Null);
                Assert.That(response.IsSuccessStatusCode, Is.True);

                var jsonString = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<dynamic>(jsonString);

                Assert.That(responseData, Is.Not.Null);
                Assert.That(responseData?.TableEntries, Is.Not.Null);
                Assert.That(responseData?.TableEntries, Is.AssignableTo<IEnumerable<dynamic>>());
            }
        }
    }
}
