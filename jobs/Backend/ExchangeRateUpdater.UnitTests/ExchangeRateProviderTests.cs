using ExchangeRateUpdater.ApiClients.CzechNationalBank;
using ExchangeRateUpdater.Models.Behavior;
using ExchangeRateUpdater.Models.Errors;
using ExchangeRateUpdater.Models.Types;
using ExchangeRateUpdater.Persistence;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.UnitTests.Mocks;
using ExchangeRateUpdater.UnitTests.Utilities;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.UnitTests
{
    public class ExchangeRateProviderTests
    {

        private readonly IExchangeRateProvider _sut;
        private readonly Mock<IExchangeRateApiClient> _exchangeRateApiClientMock;
        private readonly Mock<IExchangeRateRepository> _repositoryMock;

        public ExchangeRateProviderTests()
        {
            _exchangeRateApiClientMock = new();
            _sut = new ExchangeRateProvider(_exchangeRateApiClientMock.Object);
            _repositoryMock = RepositoryMocks.GetExchangeRateRepository();
        }

        [Fact]
        public async Task GetExchangeRates_ShouldReturnExchangeRates_WhenSourceCurrenciesAreProvided_AndApiResponsesAreSuccessful()
        {
            var apiResponseDaily = ExchangeRateApiResponses.GetSuccessfulResponse(currencyCode: "USD", amount: 1, rate: 21.9m);
            var apiResponseOther = ExchangeRateApiResponses.GetSuccessfulResponse(currencyCode: "KES", amount: 100, rate: 15.54m);
            var expectedResult = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency(new Code("USD")),new Currency(new Code("CZK")), new Rate(21.9m).GetByAmount(1)),
                new ExchangeRate(new Currency(new Code("KES")),new Currency(new Code("CZK")), new Rate(15.54m).GetByAmount(100)),
            };
            _exchangeRateApiClientMock.Setup(c => c.GetDaily()).ReturnsAsync(apiResponseDaily);
            _exchangeRateApiClientMock.Setup(c => c.GetOtherByYearMonth(It.IsAny<string>())).ReturnsAsync(apiResponseOther);
            var currencies = _repositoryMock.Object.GetSourceCurrencies();

            var result = await _sut.GetExchangeRates(currencies);

            result.AsT0.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetExchangeRates_ShouldReturnValidationError_WhenSourceCurrenciesAreNotProvided()
        {
            var expectedResult = new Error(ErrorType.ValidationError)
                .WithMessage("Source currencies list not provided while getting exchange rates.");

            var result = await _sut.GetExchangeRates(Enumerable.Empty<Currency>());

            result.AsT1.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetExchangeRates_ShouldReturnApiError_WhenApiDailyResponseWasNotSuccessful()
        {
            var apiResponse = await ExchangeRateApiResponses.GetUnsuccessfulResponse();
            var expectedResult = new Error(ErrorType.ApiError)
                .WithMessage(ExchangeRateApiResponses.GetUnsuccessfulResponseMessage());
            _exchangeRateApiClientMock.Setup(c => c.GetDaily()).ReturnsAsync(apiResponse);
            var currencies = _repositoryMock.Object.GetSourceCurrencies();

            var result = await _sut.GetExchangeRates(currencies);

            result.AsT1.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetExchangeRates_ShouldReturnApiError_WhenApiOtherResponseWasNotSuccessful()
        {
            var apiResponseDaily = ExchangeRateApiResponses.GetSuccessfulResponse(currencyCode: "USD", amount: 1, rate: 21.9m);
            var apiResponseOther = await ExchangeRateApiResponses.GetUnsuccessfulResponse();
            var expectedResult = new Error(ErrorType.ApiError)
                .WithMessage(ExchangeRateApiResponses.GetUnsuccessfulResponseMessage());
            _exchangeRateApiClientMock.Setup(c => c.GetDaily()).ReturnsAsync(apiResponseDaily);
            _exchangeRateApiClientMock.Setup(c => c.GetOtherByYearMonth(It.IsAny<string>())).ReturnsAsync(apiResponseOther);
            var currencies = _repositoryMock.Object.GetSourceCurrencies();

            var result = await _sut.GetExchangeRates(currencies);

            result.AsT1.Should().BeEquivalentTo(expectedResult);
        }
    }
}
