using ExchangeRateUpdater.Infrastructure.BusinessLogic;
using ExchangeRateUpdater.Model.Common;
using ExchangeRateUpdater.Model.Configuration;
using ExchangeRateUpdater.Model.Dto;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdater.Test
{
    [TestFixture]
    public class RateProviderTests
    {
        private ExchangeRateProvider _providerMock;
        private Mock<ICnbConfig> _cnbConfigMock;
        private Mock<ILogger<ExchangeRateProvider>> _logger;

        private ExchangeRateRequestDto _rateRequest = new();
        private ExchangeRateResponseDto _rateResponse = new();

        [SetUp]
        public void Setup()
        {
            _cnbConfigMock = new Mock<ICnbConfig>();
            _logger = new Mock<ILogger<ExchangeRateProvider>>();
            _providerMock = new ExchangeRateProvider(_cnbConfigMock.Object, _logger.Object);
        }

        [Test]
        public async Task GetExchangeRates_HappyPath()
        {
            _rateRequest = new ExchangeRateRequestDto { Currencies = GetTestCurrencies() };

            // Given
            _cnbConfigMock.SetupGet(c => c.BaseAddress).Returns("https://api.cnb.cz");
            _cnbConfigMock.SetupGet(c => c.ApiEndpoint).Returns("cnbapi/exrates/daily?");
            _cnbConfigMock.SetupGet(c => c.TargetCurrency).Returns("CZK");

            // When
            _rateResponse = await _providerMock.GetExchangeRatesAsync(_rateRequest);

            // Then
            _rateResponse.Rates.Should().NotBeNullOrEmpty()
                .And.HaveCount(5);
        }

        [TestCase(1, "ISK", "CZK")]
        [TestCase(2, "USD", "MXN")]
        [TestCase(3, "AUD", "EUR", "GBP")]
        [TestCase(4, "BGN", "AUD", "USD", "XXX", "INR")]
        [TestCase(5, "ABC", "KRW", "NOK", "SEK", "ISK", "DKK")]
        public async Task GetExchangeRates_Given_Currencies(int expRateCount, params string[] currencies)
        {
            // Given
            _rateRequest.Currencies = currencies.Select(c => new Currency(c));

            _cnbConfigMock.SetupGet(c => c.BaseAddress).Returns("https://api.cnb.cz");
            _cnbConfigMock.SetupGet(c => c.ApiEndpoint).Returns("cnbapi/exrates/daily?");
            _cnbConfigMock.SetupGet(c => c.TargetCurrency).Returns("CZK");

            // When
            _rateResponse = await _providerMock.GetExchangeRatesAsync(_rateRequest);

            // Then
            _rateResponse.Rates.Should().NotBeNullOrEmpty()
                .And.HaveCount(expRateCount);
        }

        [Test]
        public async Task GetExchangeRates_Null_Request()
        {
            // Given
            _cnbConfigMock.SetupGet(c => c.BaseAddress).Returns("https://api.cnb.cz");
            _cnbConfigMock.SetupGet(c => c.ApiEndpoint).Returns("cnbapi/exrates/daily?");
            _cnbConfigMock.SetupGet(c => c.TargetCurrency).Returns("CZK");

            // When
            _rateResponse = await _providerMock.GetExchangeRatesAsync(null);

            // Then
            _rateResponse.Rates.Should().BeNullOrEmpty();
            _rateResponse.ErrorMessage.Should().Be("Request can not be null or empty.");
        }

        private static List<Currency> GetTestCurrencies()
        {
            return new List<Currency>
            {
                new("USD"),
                new("EUR"),
                new("CZK"),
                new("JPY"),
                new("KES"),
                new("RUB"),
                new("THB"),
                new("TRY"),
                new("XYZ")
            };
        }
    }
}