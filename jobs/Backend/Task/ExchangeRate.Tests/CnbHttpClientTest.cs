using ExchangeRateProvider;
using ExchangeRateProvider.Constants;
using ExchangeRateProvider.Models;
using Moq;
using Moq.Protected;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRate.Tests
{
    public class CnbHttpClientTest
    {
        [Fact]
        public async Task GetCzkExchangeRatesAsync_WhenHttpResponseIsSuccessful_ReturnsRates()
        {
            // Arrange
            var expectedDateTime = DateTime.UtcNow;
            var expectedLanguage = Language.Czech;
            var expectedUrl = $"https://api.cnb.cz/cnbapi/exrates/daily?date={expectedDateTime:yyyy-MM-dd}&lang={expectedLanguage}";
            var expectedCurrencyCode = Currency.Euro;
            var expectedRates = new CnbRatesModel
            {
                Rates =
                [
                    new CnbRatesModel.RateModel
                    {
                        Amount = 1,
                        Country = "EMU",
                        Currency = "euro",
                        CurrencyCode = expectedCurrencyCode,
                        Order = 66,
                        Rate = 25.31M,
                        ValidFor = DateTime.UtcNow
                    }
                ]
            };
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(@"{
                    ""rates"": [{
                        ""validFor"": ""2024-04-04"",
                        ""order"": 66,
                        ""country"": ""EMU"",
                        ""currency"": ""euro"",
                        ""amount"": 1,
                        ""currencyCode"": ""EUR"",
                        ""rate"": 25.31
                    }]}")
            };
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(expectedResponse);
            var httpClient = new HttpClient(handler.Object);
            var cnbHttpClient = new CnbHttpClient(httpClient);

            // Act
            var result = await cnbHttpClient.GetCzkExchangeRatesAsync(expectedDateTime, expectedLanguage);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Rates);
            Assert.Single(result.Rates);
            var rate = result.Rates.First();
            Assert.Equal(expectedCurrencyCode, rate.CurrencyCode);
        }

        [Fact]
        public async Task GetCzkExchangeRatesAsync_WhenHttpResponseIsNotSuccessful_ThrowsException()
        {
            // Arrange
            var expectedDateTime = DateTime.UtcNow;
            var expectedLanguage = Language.Czech;
            var expectedUrl = $"https://api.cnb.cz/cnbapi/exrates/daily?date={expectedDateTime:yyyy-MM-dd}&lang={expectedLanguage}";
            var expectedResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                   .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                   .ReturnsAsync(expectedResponse);
            var httpClient = new HttpClient(handler.Object);
            var cnbHttpClient = new CnbHttpClient(httpClient);

            // Act
            // Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => cnbHttpClient.GetCzkExchangeRatesAsync(expectedDateTime, expectedLanguage));
        }
    }
}