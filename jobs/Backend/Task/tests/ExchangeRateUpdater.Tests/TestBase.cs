using System;
using System.Net.Http;
using NSubstitute;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    /// <summary>
    /// Base class for Tests
    /// </summary>
    public abstract class TestBase
    {

        public IHttpClientFactory CreateHttpClientFactoryMock(Func<HttpMessageHandler> messageHandlerFactory)
        {
            var httpClientFactoryMock = Substitute.For<IHttpClientFactory>();
            var fakeHttpClient = new HttpClient(messageHandlerFactory());
            httpClientFactoryMock.CreateClient().Returns(fakeHttpClient);
            return httpClientFactoryMock;
        }
    }
}