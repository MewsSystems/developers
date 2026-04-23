using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private static readonly Currency Czk = new Currency("CZK");
        private static readonly Currency Eur = new Currency("EUR");
        private static readonly Currency Usd = new Currency("USD");

        private static readonly IReadOnlyCollection<ExchangeRate> BasicExchangeRates = new List<ExchangeRate>
        {
            new ExchangeRate(Czk, Usd, 23.10m),
            new ExchangeRate(Czk, Eur, 25.10m)
        };

        private static readonly IReadOnlyCollection<ExchangeRate> ExchangeRatesWithOppositeDirection = new List<ExchangeRate>
        {
            new ExchangeRate(Czk, Usd, 23.10m),
            new ExchangeRate(Czk, Eur, 25.10m),
            new ExchangeRate(Usd, Czk, 1 / 23.10m),
            new ExchangeRate(Eur, Czk, 1 / 25.10m)
        };

        private readonly Mock<ICurrencyRateProvider> _currencyRateProviderMock = new();
        private readonly ExchangeRateProvider _provider;

        public ExchangeRateProviderTests()
        {
            _provider = new ExchangeRateProvider(_currencyRateProviderMock.Object);
        }

        [Fact]
        public async Task ForTwoCurrenciesAndAvailableExchangeRate_ShouldReturnCorrectExchangeRate()
        {
            _currencyRateProviderMock
                .Setup(c => c.GetExchangeRatesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(BasicExchangeRates));

            var rates = await _provider
                .GetExchangeRatesAsync([Czk, Eur], default)
                .ToListAsync();

            var expectedRate =
                BasicExchangeRates.Single(x => x.SourceCurrency.Equals(Czk) && x.TargetCurrency.Equals(Eur));

            // expecting correct output for two given currencies
            rates.Should().ContainSingle(x => x.SourceCurrency.Equals(Czk) && x.TargetCurrency.Equals(Eur));
            var rate = rates.Single(x => x.SourceCurrency.Equals(Czk) && x.TargetCurrency.Equals(Eur));
            rate.Value.Should().BeApproximately(expectedRate.Value, 0.0001m);

            // expecting not to contain currency we didn't ask for
            rates.Should().NotContain(x => x.SourceCurrency.Equals(Czk) && x.TargetCurrency.Equals(Usd));
            // expecting not to contain opposite conversion direction when it is not contained in source data
            rates.Should().NotContain(x => x.SourceCurrency.Equals(Eur) && x.TargetCurrency.Equals(Czk));
        }

        [Fact]
        public async Task ForTwoCurrenciesAndEmptySource_ShouldReturnEmptyExchangeRates()
        {
            _currencyRateProviderMock
                .Setup(c => c.GetExchangeRatesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IReadOnlyCollection<ExchangeRate>>([]));

            var rates = await _provider.GetExchangeRatesAsync([Czk, Eur], default).ToListAsync();

            rates.Should().BeEmpty();
        }

        [Fact]
        public async Task ForUnknownCurrency_ShouldNotCauseExceptionAndBeIgnored()
        {
            _currencyRateProviderMock
                .Setup(c => c.GetExchangeRatesAsync(default))
                .Returns(Task.FromResult(BasicExchangeRates));

            var rates = await _provider.GetExchangeRatesAsync([Czk, new Currency("XYZ")], default).ToListAsync();
            rates.Should().BeEmpty();
        }

        [Fact]
        public async Task BidirectionalRatesInSource_RatesForBothDirectionsShouldBeReturned()
        {
            _currencyRateProviderMock
                .Setup(c => c.GetExchangeRatesAsync(default))
                .Returns(Task.FromResult(ExchangeRatesWithOppositeDirection));

            var rates = await _provider.GetExchangeRatesAsync([Czk, Usd], default).ToListAsync();

            rates.Should().HaveCount(2);
            rates.Should().Contain(x => x.SourceCurrency.Equals(Czk) && x.TargetCurrency.Equals(Usd));
            rates.Should().Contain(x => x.SourceCurrency.Equals(Usd) && x.TargetCurrency.Equals(Czk));
        }
    }
}