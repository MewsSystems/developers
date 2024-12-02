using API.Models;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Provider.CzechNationalBank;
using System.Net;

namespace API.UnitTests
{
    public class CzechNationalBankExchangeRateProviderTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly CzechNationalBankExchangeRateProvider _provider;

        public CzechNationalBankExchangeRateProviderTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _provider = new CzechNationalBankExchangeRateProvider(_httpClient);
        }

        [Fact]
        public async Task GetCurrentExchangeRatesAsync_ShouldReturnExchangeRates_ForValidCurrencies()
        {
            // Arrange
            var currencies = new List<Currency> { new("GBP"), new("EUR") };
            var jsonResponse = @"
            {
                'rates': [
                    { 'currencyCode': 'GBP', 'rate': 29.013 },
                    { 'currencyCode': 'EUR', 'rate': 24.73 }
                ]
            }";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            var result = await _provider.GetCurrentExchangeRatesAsync(currencies, cancellationToken);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(rate => rate.TargetCurrency.Code == "GBP" && rate.Value == 29.013m);
            result.Should().Contain(rate => rate.TargetCurrency.Code == "EUR" && rate.Value == 24.73m);
        }

        [Fact]
        public async Task GetCurrentExchangeRatesAsync_ShouldReturnPartialExchangeRates_ForValidCurrencies()
        {
            // Arrange
            var currencies = new List<Currency> { new("GBP"), new("EUR") };
            var jsonResponse = @"
            {
                'rates': [
                    { 'currencyCode': 'GBP', 'rate': 29.013 },
                ]
            }";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            var result = await _provider.GetCurrentExchangeRatesAsync(currencies, cancellationToken);

            // Assert
            result.Should().HaveCount(1);
            result.Should().Contain(rate => rate.TargetCurrency.Code == "GBP" && rate.Value == 29.013m);
        }

        [Fact]
        public async Task GetCurrentExchangeRatesAsync_ShouldReturnEmpty_WhenCurrencyNotFound()
        {
            // Arrange
            var currencies = new List<Currency> { new("XYZ") };
            var jsonResponse = @"
            {
                'rates': [
                    { 'currencyCode': 'GBP', 'rate': 29.013 },
                    { 'currencyCode': 'EUR', 'rate': 24.73 }
                ]
            }";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            var result = await _provider.GetCurrentExchangeRatesAsync(currencies, cancellationToken);

            // Assert
            result.Should().BeEmpty();
        }
    }
}