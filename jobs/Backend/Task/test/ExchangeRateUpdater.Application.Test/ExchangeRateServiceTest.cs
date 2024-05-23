using ExchangeRateUpdater.Application.Models;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.UseCases;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ExchangeRateUpdater.Application.Test
{
    public class ExchangeRateServiceTest
    {
        private readonly Mock<IGetDailyExchangeRateUseCase> _getDailyExchangeRateUseCaseMock;

        public ExchangeRateServiceTest()
        {
            _getDailyExchangeRateUseCaseMock = new Mock<IGetDailyExchangeRateUseCase>();
        }

        [Fact]
        public void ExchangeRateService_GetDailyExchangeRateForCurrencies_TargetCurrencyNull_ShoudThrowArgumentException()
        {
            var sut = new ExchangeRateService(_getDailyExchangeRateUseCaseMock.Object, new NullLogger<ExchangeRateService>());

            Action serviceCall = () => sut.GetDailyExchangeRateForCurrencies(null, new CurrencyDto[] { "ABC" }, default(CancellationToken)).Wait();

            serviceCall.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ExchangeRateService_GetDailyExchangeRateForCurrencies_SoruceCurrenciesNull_ShoudThrowArgumentException()
        {
            var sut = new ExchangeRateService(_getDailyExchangeRateUseCaseMock.Object, new NullLogger<ExchangeRateService>());

            Action serviceCall = () => sut.GetDailyExchangeRateForCurrencies("ABC", null, default(CancellationToken)).Wait();

            serviceCall.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ExchangeRateService_GetDailyExchangeRateForCurrencies_SourceCurrencyEmpty_ShoudThrowArgumentException()
        {
            var sut = new ExchangeRateService(_getDailyExchangeRateUseCaseMock.Object, new NullLogger<ExchangeRateService>());

            Action serviceCall = () => sut.GetDailyExchangeRateForCurrencies("ABC", Enumerable.Empty<CurrencyDto>(), default(CancellationToken)).Wait();

            serviceCall.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ExchangeRateService_GetDailyExchangeRateForCurrencies_UseCaseFails_ShoudThrowException()
        {
            _getDailyExchangeRateUseCaseMock.Setup(x => x.ExecuteAsync(It.IsAny<Currency>(), It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
                .Throws<Exception>();

            var sut = new ExchangeRateService(_getDailyExchangeRateUseCaseMock.Object, new NullLogger<ExchangeRateService>());

            Action serviceCall = () => sut.GetDailyExchangeRateForCurrencies("ABC", new CurrencyDto[] { "CDE" }, default(CancellationToken)).Wait();

            serviceCall.Should().Throw<Exception>();
        }

        [Fact]
        public async Task ExchangeRateService_GetDailyExchangeRateForCurrencies_UseCaseReturnNull_ShouldReturnEmptyExchangeRateDto()
        {
            _getDailyExchangeRateUseCaseMock.Setup(x => x.ExecuteAsync(It.IsAny<Currency>(), It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<ExchangeRate>)null);

            var sut = new ExchangeRateService(_getDailyExchangeRateUseCaseMock.Object, new NullLogger<ExchangeRateService>());

            var exchangeRates = await sut.GetDailyExchangeRateForCurrencies("ABC", new CurrencyDto[] { "CDE", "FGH", "XXX" }, default(CancellationToken));

            exchangeRates.Should().BeEmpty();
        }

        [Fact]
        public async Task ExchangeRateService_GetDailyExchangeRateForCurrencies_UseCaseReturnEmptyList_ShouldReturnEmptyExchangeRateDto()
        {
            _getDailyExchangeRateUseCaseMock.Setup(x => x.ExecuteAsync(It.IsAny<Currency>(), It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<ExchangeRate>());

            var sut = new ExchangeRateService(_getDailyExchangeRateUseCaseMock.Object, new NullLogger<ExchangeRateService>());

            var exchangeRates = await sut.GetDailyExchangeRateForCurrencies("ABC", new CurrencyDto[] { "CDE", "FGH", "XXX" }, default(CancellationToken));

            exchangeRates.Should().BeEmpty();
        }

        [Fact]
        public async Task ExchangeRateService_GetDailyExchangeRateForCurrencies_ValidUseCaseReturn_ShouldReturnExchangeRateDto()
        {
            _getDailyExchangeRateUseCaseMock.Setup(x => x.ExecuteAsync(It.IsAny<Currency>(), It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[]
                {
                    ExchangeRate.Create(Currency.Create("ABC"), Currency.Create("CDE"), 2),
                    ExchangeRate.Create(Currency.Create("ABC"), Currency.Create("XXX"), 3)
                });

            var sut = new ExchangeRateService(_getDailyExchangeRateUseCaseMock.Object, new NullLogger<ExchangeRateService>());

            var exchangeRates = await sut.GetDailyExchangeRateForCurrencies("ABC", new CurrencyDto[] { "CDE", "FGH", "XXX" }, default(CancellationToken));

            exchangeRates.Should().NotBeNullOrEmpty();
            exchangeRates.Should().HaveCount(2);
            exchangeRates.Should().BeAssignableTo(typeof(IEnumerable<ExchangeRateDto>));
        }
    }
}
