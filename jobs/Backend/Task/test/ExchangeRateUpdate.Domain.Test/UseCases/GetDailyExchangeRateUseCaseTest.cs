using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.UseCases;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdate.Domain.Test.UseCases
{
    public class GetDailyExchangeRateUseCaseTest
    {
        private readonly Mock<IExchangeRateProvider> _exchangeRateProviderMock;
        private readonly Mock<ILogger<GetDailyExchangeRateUseCase>> _logger;

        public GetDailyExchangeRateUseCaseTest()
        {
            _exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
            _logger = new Mock<ILogger<GetDailyExchangeRateUseCase>>();
        }

        [Fact]
        public void GetDailyExchangeRateUseCase_ExecuteAsync_WhenTargetCurrenciesIsNull_ShouldThrowArgumentException()
        {
            var sut = new GetDailyExchangeRateUseCase(_exchangeRateProviderMock.Object, _logger.Object);

            Action useCase = () => sut.ExecuteAsync(Currency.Create("ABC"), null, default(CancellationToken)).Wait();

            useCase.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDailyExchangeRateUseCase_ExecuteAsync_WhenTargetCurrenciesIsEmpty_ShouldThrowArgumentException()
        {
            var sut = new GetDailyExchangeRateUseCase(_exchangeRateProviderMock.Object, _logger.Object);

            Action useCase = () => sut.ExecuteAsync(Currency.Create("ABC"), Enumerable.Empty<Currency>(), default(CancellationToken)).Wait();

            useCase.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDailyExchangeRateUseCase_ExecuteAsync_WhenSourceCurrencyIsNull_ShouldThrowArgumentException()
        {
            var sut = new GetDailyExchangeRateUseCase(_exchangeRateProviderMock.Object, _logger.Object);

            Action useCase = () => sut.ExecuteAsync(null, new[] { Currency.Create("ABC") }, default(CancellationToken)).Wait();

            useCase.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetDailyExchangeRateUseCase_ExecuteAsync_WhenProviderFails_ShouldThrowAnException()
        {
            _exchangeRateProviderMock.Setup(x => x.GetDailyExchangeRates(It.IsAny<Currency>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var sut = new GetDailyExchangeRateUseCase(_exchangeRateProviderMock.Object, _logger.Object);

            Action useCase = () => sut.ExecuteAsync(Currency.Create("CDE"), new[] { Currency.Create("ABC") }, default(CancellationToken)).Wait();

            useCase.Should().Throw<Exception>();
        }

        [Fact]
        public async Task GetDailyExchangeRateUseCase_ExecuteAsync_WhenProviderReturnsData_ShouldFilterCurrencies()
        {
            var exchangeRatesResponse = new[]
            {
                ExchangeRate.Create(Currency.Create("FGH"), Currency.Create("CDE"), 3)
            };

            _exchangeRateProviderMock.Setup(x => x.GetDailyExchangeRates(It.IsAny<Currency>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exchangeRatesResponse);

            var sut = new GetDailyExchangeRateUseCase(_exchangeRateProviderMock.Object, _logger.Object);

            var exchangeRates = await sut.ExecuteAsync(Currency.Create("FGH"), new[] { Currency.Create("ABC"), Currency.Create("CDE") }, default(CancellationToken));

            exchangeRates.Should().NotBeNullOrEmpty();
            exchangeRates.Should().HaveCount(1);
            
            var exchangeRate = exchangeRates.FirstOrDefault();
            exchangeRate.SourceCurrency.Code.Should().Be("FGH");
            exchangeRate.TargetCurrency.Code.Should().Be("CDE");
            exchangeRate.Value.Should().Be(3);
        }

        [Fact]
        public async Task GetDailyExchangeRateUseCase_ExecuteAsync_WhenProviderReturnsNull_ShouldReturnEmptyList()
        {
            _exchangeRateProviderMock.Setup(x => x.GetDailyExchangeRates(It.IsAny<Currency>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<ExchangeRate>)null);

            var sut = new GetDailyExchangeRateUseCase(_exchangeRateProviderMock.Object, _logger.Object);

            var exchangeRates = await sut.ExecuteAsync(Currency.Create("FGH"), new[] { Currency.Create("ABC"), Currency.Create("CDE") }, default(CancellationToken));

            exchangeRates.Should().BeEmpty();
        }

        [Fact]
        public async Task GetDailyExchangeRateUseCase_ExecuteAsync_WhenProviderEmptyList_ShouldReturnEmptyList()
        {
            _exchangeRateProviderMock.Setup(x => x.GetDailyExchangeRates(It.IsAny<Currency>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<ExchangeRate>());

            var sut = new GetDailyExchangeRateUseCase(_exchangeRateProviderMock.Object, _logger.Object);

            var exchangeRates = await sut.ExecuteAsync(Currency.Create("FGH"), new[] { Currency.Create("ABC"), Currency.Create("CDE") }, default(CancellationToken));

            exchangeRates.Should().BeEmpty();
        }
    }
}
