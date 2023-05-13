using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Infrastructure.ExternalServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdaterTests.Infrastructure
{
    public class CnbExchangeRateProviderTests
    {
        const string SourceCurrency = "CZK";

        [Fact]
        public async Task GetExchangeRatesAsync_When_CurrenciesIsNull_Then_ThrowsArgumentException()
        {
            // Arrange
            var rateProvider = new CnbExchangeRateProvider(new HttpClient(), SourceCurrency);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await rateProvider.GetExchangeRatesAsync(null!));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_When_CurrenciesIsEmpty_Then_ThrowsArgumentException()
        {
            // Arrange
            var rateProvider = new CnbExchangeRateProvider(new HttpClient(), SourceCurrency);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await rateProvider.GetExchangeRatesAsync(Enumerable.Empty<Currency>()));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_When_ApiReturnsNotSuccessCode_Then_ThrowsHttpRequestException()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("USD") };
            var httpClient = GetHttpClient(HttpStatusCode.NotFound, new StringContent(""));

            var rateProvider = new CnbExchangeRateProvider(httpClient, SourceCurrency);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException> (async () => await rateProvider.GetExchangeRatesAsync(currencies));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_When_ApiReturnsEmptyContent_Then_ReturnsEmptyExchangeRates()
        {
            // Arrange
            var currencies = new List<Currency> { new Currency("USD") };
            var httpClient = GetHttpClient(HttpStatusCode.OK, new StringContent(""));

            var rateProvider = new CnbExchangeRateProvider(httpClient, SourceCurrency);

            // Act
            var exchangeRates = await rateProvider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Empty(exchangeRates);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_When_ApiReturnsExchangeRates_Then_ReturnsFilteredExchangeRates()
        {
            // Arrange
            var rawContent = File.ReadAllText(@"Files\ExchangeRates.txt");
            var content = new StringContent(rawContent);

            var currencies = new List<Currency> { new Currency("USD"), new Currency("INR"), new Currency("SEK"), new Currency("IGS") };
            var httpClient = GetHttpClient(HttpStatusCode.OK, content);

            var rateProvider = new CnbExchangeRateProvider(httpClient, SourceCurrency);

            // Act
            var exchangeRates = await rateProvider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.NotNull(exchangeRates);
            Assert.NotEmpty(exchangeRates);
            Assert.Equal(3, exchangeRates.Count());
            Assert.Contains(exchangeRates!, er => er.TargetCurrency.Code == "USD");
            Assert.Contains(exchangeRates!, er => er.TargetCurrency.Code == "INR");
            Assert.Contains(exchangeRates!, er => er.TargetCurrency.Code == "SEK");
        }

        private static HttpClient GetHttpClient(HttpStatusCode statusCode, StringContent content)
        {
            return new HttpClient(new HttpMessageHandlerStub(async (request, cancellationToken) =>
            {
                var responseMessage = new HttpResponseMessage(statusCode)
                {
                    Content = content
                };

                return await Task.FromResult(responseMessage);
            }))
            {
                BaseAddress = new Uri("http://fake.url")
            };
        }
    }

    internal class HttpMessageHandlerStub : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendAsync;

        public HttpMessageHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync)
        {
            _sendAsync = sendAsync;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await _sendAsync(request, cancellationToken);
        }
    }
}
