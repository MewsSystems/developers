using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Data.Models;
using ExchangeRateUpdater.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace ExchangeRateUpdaterTest
{
    public class CnbExchangeRateProviderTest
    {
        private readonly Mock<ILogger<CnbExchangeRateProvider>> _loggerMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private CnbExchangeRateProvider? _cnbExchangeRateProvider;

        public CnbExchangeRateProviderTest()
        {
            _loggerMock = new Mock<ILogger<CnbExchangeRateProvider>>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        }

        [Fact]
        public async void GetExchangeRatesAsync_AllOk()
        {
            //Arrenge
            List<Currency> currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            var cnbResposnse = CreateValidCnbResponse();
            var responseMessage = CreateValidClientResponse(cnbResposnse);
            
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage)
                .Verifiable();
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(_handlerMock.Object));
            _cnbExchangeRateProvider = new CnbExchangeRateProvider(_httpClientFactoryMock.Object, _loggerMock.Object);

            //Act
            var exchangeRates = await _cnbExchangeRateProvider.GetExchangeRatesAsync(currencies);

            //Assert
            exchangeRates.Should().NotBeNull();
            exchangeRates.Should().HaveCount(2);
            exchangeRates.Should().Contain(x => x.SourceCurrency.Code == "CZK" &&
            x.TargetCurrency.Code == cnbResposnse.Rates[0].CurrencyCode &&
            x.Value == Math.Round(cnbResposnse.Rates[0].Rate, 2));
            exchangeRates.Should().Contain(x => x.SourceCurrency.Code == "CZK" &&
            x.TargetCurrency.Code == cnbResposnse.Rates[1].CurrencyCode &&
            x.Value == Math.Round(cnbResposnse.Rates[1].Rate, 2));
        }

        [Fact]
        public async void GetExchangeRatesAsync_OnError_RetryAndLog()
        {
            //Arrenge
            var errorMessage = "Test error message";
            var responseMessage = CreateErrorClientResponse(errorMessage);

            _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage)
            .Verifiable();
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(_handlerMock.Object));
            _cnbExchangeRateProvider = new CnbExchangeRateProvider(_httpClientFactoryMock.Object, _loggerMock.Object);

            //Act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () => await _cnbExchangeRateProvider.GetExchangeRatesAsync(null));
            

            //Assert
            _handlerMock.Protected().Verify("SendAsync", Times.Exactly(4), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(3));
            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void GetExchangeRatesAsync_SuccedsAfterRetry()
        {
            //Arrenge
            List<Currency> currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            var errorMessage = "Test error message";
            var cnbResposnse = CreateValidCnbResponse();
            var responseMessage = CreateValidClientResponse(cnbResposnse);

            _handlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(CreateErrorClientResponse(errorMessage))
                .ReturnsAsync(responseMessage);
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(_handlerMock.Object));
            _cnbExchangeRateProvider = new CnbExchangeRateProvider(_httpClientFactoryMock.Object, _loggerMock.Object);

            //Act
            var exchangeRates = await _cnbExchangeRateProvider.GetExchangeRatesAsync(currencies);

            //Assert
            exchangeRates.Should().NotBeNull();
            exchangeRates.Should().HaveCount(2);
            exchangeRates.Should().Contain(x => x.SourceCurrency.Code == "CZK" &&
            x.TargetCurrency.Code == cnbResposnse.Rates[0].CurrencyCode &&
            x.Value == Math.Round(cnbResposnse.Rates[0].Rate, 2));
            exchangeRates.Should().Contain(x => x.SourceCurrency.Code == "CZK" &&
            x.TargetCurrency.Code == cnbResposnse.Rates[1].CurrencyCode &&
            x.Value == Math.Round(cnbResposnse.Rates[1].Rate, 2));

            _handlerMock.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(1));
        }

        private CnbDailyRatesResponse CreateValidCnbResponse()
        {
            return new CnbDailyRatesResponse()
            {
                Rates = new List<CnbRate>
                {
                    new CnbRate()
                    {
                        CurrencyCode = "USD",
                        Rate = 14.857M,
                    },
                    new CnbRate()
                    {
                        CurrencyCode = "EUR",
                        Rate = 18,
                    }
                }
            };
        }

        private HttpResponseMessage CreateValidClientResponse(CnbDailyRatesResponse cnbResponse)
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(cnbResponse))
            };
        }

        private HttpResponseMessage CreateErrorClientResponse(string errorMessage)
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(errorMessage)
            };
        }
    }
}