using ExchangeRateUpdater.Persistence;
using ExchangeRateUpdater.Models.Errors;
using ExchangeRateUpdater.Models.Types;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using OneOf;
using FluentAssertions;

namespace ExchangeRateUpdater.UnitTests
{
    public class AppTests
    {
        private readonly Mock<IExchangeRateRepository> _repositoryMock;
        private readonly Mock<IExchangeRateProvider> _exchangeRateProviderMock;
        private readonly Mock<ILogger<App>> _loggerMock;
        private readonly App _sut;

        public AppTests()
        {
            _repositoryMock = RepositoryMocks.GetExchangeRateRepository();
            _exchangeRateProviderMock = new();
            _loggerMock = new();
            _sut = new App(_loggerMock.Object, _repositoryMock.Object, _exchangeRateProviderMock.Object);
        }

        [Fact]
        public async Task Run_ShouldReturnZero_WhenExchangeRateProviderSuccessfullyReturnsItems()
        {

            _exchangeRateProviderMock.Setup(e => e.GetExchangeRates(It.IsAny<IEnumerable<Currency>>()))
                .ReturnsAsync(OneOf<IEnumerable<ExchangeRate>, Error>.FromT0(Enumerable.Empty<ExchangeRate>()));

            var result = await _sut.Run(Array.Empty<string>());

            result.Should().Be(0);
        }

        [Fact]
        public async Task Run_ShouldReturnMinusOne_WhenExchangeRateProviderReturnsValidationError()
        {
            _exchangeRateProviderMock.Setup(e => e.GetExchangeRates(It.IsAny<IEnumerable<Currency>>()))
                .ReturnsAsync(new Error(ErrorType.ValidationError));

            var result = await _sut.Run(Array.Empty<string>());

            result.Should().Be(-1);
        }

        [Fact]
        public async Task Run_ShouldReturnMinusTwo_WhenAppThrowsException()
        {
            _exchangeRateProviderMock.Setup(e => e.GetExchangeRates(It.IsAny<IEnumerable<Currency>>()))
                .ThrowsAsync(new Exception());

            var result = await _sut.Run(Array.Empty<string>());

            result.Should().Be(-2);

        }
    }
}