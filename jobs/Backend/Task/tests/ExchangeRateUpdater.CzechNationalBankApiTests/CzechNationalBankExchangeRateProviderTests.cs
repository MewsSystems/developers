using AutoFixture;
using ExchangeRateUpdater.Core.Models.CzechNationalBank;
using ExchangeRateUpdater.CzechNationalBank.Api;
using ExchangeRateUpdater.CzechNationalBankTests.Data;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.CzechNationalBankApiTests
{
    public class CzechNationalBankExchangeRateProviderTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<ICzechNationalBankApi> _czechNationalBankApiMock;
        private readonly Mock<ILogger<CzechNationalBankExchangeRateProvider>> _logger;
        private readonly IEnumerable<string> _availableCurrencies;

        private readonly CzechNationalBankExchangeRateProvider _sut;

        public CzechNationalBankExchangeRateProviderTests()
        {
            _czechNationalBankApiMock = new Mock<ICzechNationalBankApi>(MockBehavior.Strict);
            _logger = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();
            _availableCurrencies = _fixture.CreateMany<string>();

            _sut = new CzechNationalBankExchangeRateProvider(_czechNationalBankApiMock.Object, _logger.Object);
        }

        [Fact]
        public async void GetExchangeRatesAsync_ResponseIsNull_ReturnsEmpty()
        {
            #region Arrange
            var exchangeRatesDaily = _fixture.Create<ExchangeRatesDailyDto>();
            var emptyResult = new List<ExchangeRate>();

            _czechNationalBankApiMock.Setup(x => x.GetExchangeRatesAsync()).ReturnsAsync((ExchangeRatesDailyDto?)null);

            #endregion

            #region Act

            var result = await _sut.GetExchangeRatesAsync(_availableCurrencies);

            #endregion

            #region Assert

            result.Should().BeEquivalentTo(emptyResult);

            #endregion
        }

        [Fact]
        public async void GetExchangeRatesAsync_ResponseRatesAreEmpty_ReturnsEmpty()
        {
            #region Arrange
            var emptyResult = new List<ExchangeRate>();
            var exchangeRatesDaily = _fixture.Build<ExchangeRatesDailyDto>().With(x => x.Rates, new List<ExchangeRateResponse>()).Create();

            _czechNationalBankApiMock.Setup(x => x.GetExchangeRatesAsync()).ReturnsAsync((ExchangeRatesDailyDto?)exchangeRatesDaily);

            #endregion

            #region Act

            var result = await _sut.GetExchangeRatesAsync(_availableCurrencies);

            #endregion

            #region Assert

            result.Should().BeEquivalentTo(emptyResult);

            #endregion
        }

        [Fact]
        public async void GetExchangeRatesAsync_GotInvalidResponseRates_ReturnsValidExchangeRates()
        {
            #region Arrange
            var exchangeRatesDaily = _fixture.Create<ExchangeRatesDailyDto>();
            var invalidRate = _fixture.Build<ExchangeRateResponse>().With(x => x.Rate, 0).Create();
            exchangeRatesDaily.Rates.Add(invalidRate);
            var currencies = exchangeRatesDaily.Rates.Select(x => x.CurrencyCode);

            _czechNationalBankApiMock.Setup(x => x.GetExchangeRatesAsync()).ReturnsAsync((ExchangeRatesDailyDto?)exchangeRatesDaily);

            #endregion

            #region Act

            var result = await _sut.GetExchangeRatesAsync(currencies);

            #endregion

            #region Assert
            result.Count().Should().Be(exchangeRatesDaily.Rates.Count - 1);
            result.Should().NotContain(x => x.SourceCurrency.Code == invalidRate.CurrencyCode);
            #endregion
        }

        [Theory]
        [ClassData(typeof(ValidExchangeRateTestData))]
        public async void GetExchangeRatesAsync_GotResponseRates_ReturnsExchangeRates(ExchangeRateResponse exchangeRateResponse, string expectedResult)
        {
            #region Arrange
            var emptyResult = new List<ExchangeRate>();
            var exchangeRatesDaily = _fixture.Build<ExchangeRatesDailyDto>()
                .With(x => x.Rates, new List<ExchangeRateResponse>() { exchangeRateResponse }).Create();
            var currencies = exchangeRatesDaily.Rates.Select(x => x.CurrencyCode);

            _czechNationalBankApiMock.Setup(x => x.GetExchangeRatesAsync()).ReturnsAsync((ExchangeRatesDailyDto?)exchangeRatesDaily);

            #endregion

            #region Act

            var result = await _sut.GetExchangeRatesAsync(currencies);

            #endregion

            #region Assert

            result.First().ToString().Should().Be(expectedResult);

            #endregion
        }
    }
}