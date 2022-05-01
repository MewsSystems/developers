using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdaterUnitTests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task Should_return_proper_rates_if_source_currency_exists()
        {
            //Arrange
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText("Content/sampleSuccessfulResponse.txt")),
                });


            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(() =>
                {
                    client.BaseAddress = new Uri("http://mockapi.com/");
                    return client;
                });


            //Act
            var exchangeRateProvider = new ExchangeRateProvider(mockFactory.Object, new Mock<ILogger>().Object);
            var result = await exchangeRateProvider.GetExchangeRates(new List<Currency> { new Currency("USD") });

            //Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(23.344M, result.First().Value);
        }

        [Fact]
        public async Task Should_return_nothing_if_source_currency_does_not_exist()
        {
            //Arrange
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText("Content/sampleSuccessfulResponse.txt")),
                });


            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(() =>
                {
                    client.BaseAddress = new Uri("http://mockapi.com/");
                    return client;
                });


            //Act
            var exchangeRateProvider = new ExchangeRateProvider(mockFactory.Object, new Mock<ILogger>().Object);
            var result = await exchangeRateProvider.GetExchangeRates(new List<Currency> { new Currency("NA") });

            //Assert
            Assert.Empty(result);
        }
    }
}