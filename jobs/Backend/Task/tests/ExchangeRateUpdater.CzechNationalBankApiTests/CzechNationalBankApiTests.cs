using AutoFixture;
using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Core.Models.CzechNationalBank;
using ExchangeRateUpdater.CzechNationalBank.Sources;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace ExchangeRateUpdater.CzechNationalBankApiTests
{
    public class CzechNationalBankApiTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly Mock<ILogger<CzechNationalBankApi>> _loggerMock;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandlerMock;
        private readonly CzechNationalBankApi _sut;

        public CzechNationalBankApiTests()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _loggerMock = new Mock<ILogger<CzechNationalBankApi>>();

            _sut = new CzechNationalBankApi(
                _clientFactoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async void GetExchangeRatesAsync_UnsuccessfulResponse_ThrowsException()
        {
            #region Arrange

            var client = CreateMockClient(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            _clientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            #endregion

            #region Act

            var act = () => _sut.GetExchangeRatesAsync();

            #endregion

            #region Assert

            await act.Should().ThrowAsync<CzechNationalBankApiException>().WithMessage("Internal Server Error");

            #endregion
        }

        [Fact]
        public async void GetExchangeRatesAsync_InvalidResponse_ThrowsException()
        {
            #region Arrange

            var client = CreateMockClient(new HttpResponseMessage{
                Content = new StringContent(_fixture.Create<string>()),
                StatusCode = HttpStatusCode.OK
            });
            _clientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            #endregion

            #region Act

            var act = () => _sut.GetExchangeRatesAsync();

            #endregion

            #region Assert

            await act.Should().ThrowAsync<CzechNationalBankApiException>().WithMessage("Failed parsing response");

            #endregion
        }

        [Fact]
        public async void GetExchangeRatesAsync_SuccessfulResponse_ReturnsRates()
        {
            #region Arrange
            var exchangeRatesDaily = _fixture.Create<ExchangeRatesDailyDto>();
            var exchangeRatesDailyString = JsonSerializer.Serialize(exchangeRatesDaily);

            var client = CreateMockClient(new HttpResponseMessage
            {
                Content = new StringContent(exchangeRatesDailyString),
                StatusCode = HttpStatusCode.OK
            });
            _clientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            #endregion

            #region Act

            var result = await _sut.GetExchangeRatesAsync();

            #endregion

            #region Assert

            result.Should().BeEquivalentTo(exchangeRatesDaily);

            #endregion
        }

        private HttpClient CreateMockClient(HttpResponseMessage response)
        {
            _mockHttpMessageHandlerMock.Protected()
                            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                            .ReturnsAsync(response);

            var client = new HttpClient(_mockHttpMessageHandlerMock.Object);
            client.BaseAddress = _fixture.Create<Uri>();

            return client;
        }
    }
}
