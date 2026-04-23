using AutoFixture;
using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<IHttpWebClient> _mockHttpClient = new();
        private readonly Mock<ILogger<ExchangeRateProvider>> _mockLogger;
        private readonly SettingOptions _settingOptions;
        private readonly ExchangeRateProvider _provider;
        private readonly HttpResponseMessage _httpResponse;
        private readonly List<Currency> _currencies;

        public ExchangeRateProviderTests()
        {
            _mockLogger = new Mock<ILogger<ExchangeRateProvider>>();
            _httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            _settingOptions = new SettingOptions
            {
                BaseUrl = "https://api.cnb.cz",
                Endpoint = "daily.txt",
                AllowedCurrencies = new List<string> { "USD", "EUR", "CZK", "JPY" }
            };
            _provider = new ExchangeRateProvider(
                _mockHttpClient.Object,
                _mockLogger.Object);
            _currencies = _settingOptions.AllowedCurrencies.Select(x => new Currency(x)).ToList();
        }

        // Happy path tests
        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsRates_ForValidResponse()
        {
            // Arrange
            var testData = @"30.03.2024 #65
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|22.521
EMU|euro|1|EUR|24.710";

            _httpResponse.Content = new StringContent(testData, Encoding.UTF8, "text/plain");

            _mockHttpClient.Setup(x => x.GetAsync()).ReturnsAsync(_httpResponse);


            // Act
            var result = await _provider.GetExchangeRatesAsync(_currencies);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(r => r.TargetCurrency.Code == "USD" && r.Value == 22.521m);
            result.Should().Contain(r => r.TargetCurrency.Code == "EUR" && r.Value == 24.710m);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_NormalizesAmounts_WhenAmountNotOne()
        {
            // Arrange
            var testData = @"30.03.2024 #65
Country|Currency|Amount|Code|Rate
Japan|yen|100|JPY|15.324";

            _httpResponse.Content = new StringContent(testData, Encoding.UTF8, "text/plain");

            _mockHttpClient.Setup(x => x.GetAsync()).ReturnsAsync(_httpResponse);

            // Act
            var result = await _provider.GetExchangeRatesAsync(_currencies);

            // Assert
            result.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new
                {
                    SourceCurrency = new Currency("CZK"),
                    TargetCurrency = new Currency("JPY"),
                    Value = 0.1532m  // 15.32 / 100
                });
        }

        // Error handling tests
        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsEmpt_WhenAllDataInvalid()
        {

            // Arrange
            var testData = @"30.03.2024 #65
Country|Currency|Amount|Rate
Japan|yen|100|JPY";

            _httpResponse.Content = new StringContent(testData, Encoding.UTF8, "text/plain");
            _mockHttpClient.Setup(x => x.GetAsync()).ReturnsAsync(_httpResponse);

            // Act
            var result = await _provider.GetExchangeRatesAsync(_currencies);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsEmpty_WhenNoMatchingCurrencies()
        {
            // Arrange
            var testData = @"30.03.2024 #65
Country|Currency|Amount|Code|Rate
UK|pound|1|GBP|28.950"; // GBP not in allowed currencies

            _httpResponse.Content = new StringContent(testData, Encoding.UTF8, "text/plain");
            _mockHttpClient.Setup(x => x.GetAsync()).ReturnsAsync(_httpResponse);

            // Act
            var result = await _provider.GetExchangeRatesAsync(_currencies);

            // Assert
            result.Should().BeEmpty();
        }

        // HTTP behavior tests
        [Fact]
        public async Task GetExchangeRatesAsync_ThrowsHttpRequestException_OnApiError()
        {
            // Arrange
            _mockHttpClient.Setup(x => x.GetAsync()).ThrowsAsync(new HttpRequestException("API error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                () => _provider.GetExchangeRatesAsync(_currencies));
        }

        [Fact]
        public async Task GetExchangeRatesAsync_LogsError_OnFailedRequest()
        {
            // Arrange
            _mockHttpClient.Setup(x => x.GetAsync()).ThrowsAsync(new HttpRequestException("API error"));

            // Act
            try { await _provider.GetExchangeRatesAsync(_currencies); } catch { }

            // Assert
            _mockLogger.Verify(
     x => x.Log(
         LogLevel.Error,
         It.IsAny<EventId>(),
         It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("API error")),
         It.IsAny<Exception>(),
         It.IsAny<Func<It.IsAnyType, Exception, string>>()),
     Times.Once);
        }

        // Edge cases
        [Fact]
        public async Task GetExchangeRatesAsync_HandlesEmptyResponse()
        {
            // Arrange
            var testData = @"30.03.2024 #65
Country|Currency|Amount|Code|Rate";

            _httpResponse.Content = new StringContent(testData, Encoding.UTF8, "text/plain");
            _mockHttpClient.Setup(x => x.GetAsync()).ReturnsAsync(_httpResponse);

            // Act
            var result = await _provider.GetExchangeRatesAsync(_currencies);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetExchangeRatesAsync_SkipsInvalidLines()
        {
            // Arrange
            var testData = @"30.03.2024 #65
Country|Currency|Amount|Code|Rate
Invalid Line
USA|dollar|1|USD|22.521
Japan|yen|100|JPY|12.34
EMU|euro|1|EUR|24.710";

            _httpResponse.Content = new StringContent(testData, Encoding.UTF8, "text/plain");
            _mockHttpClient.Setup(x => x.GetAsync()).ReturnsAsync(_httpResponse);

            // Act
            var result = await _provider.GetExchangeRatesAsync(_currencies);

            // Assert
            result.Should().HaveCount(3);
        }
    }
}
