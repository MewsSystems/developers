using ExchangeRateUpdater.Domain.Entities;
using FluentAssertions;

namespace ExchangeRateUpdate.Domain.Test.Models
{
    public class ExchangeRateTest
    {
        [Fact]
        public void ExchangeRate_WhenCurrencySourceIsNull_ShouldThrowArgumentException()
        {
            Action instantiateCurrency = () => ExchangeRate.Create(null, Currency.Create("ABC"), 1);

            instantiateCurrency.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ExchangeRate_WhenCurrencyTargetIsNull_ShouldThrowArgumentException()
        {
            Action instantiateCurrency = () => ExchangeRate.Create(Currency.Create("ABC"), null, 1);

            instantiateCurrency.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ExchangeRate_WhenCurrencyTargetEqualsSourceCurrency_ShouldThrowArgumentException()
        {
            Action instantiateCurrency = () => ExchangeRate.Create(Currency.Create("ABC"), Currency.Create("ABC"), 1);

            instantiateCurrency.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ExchangeRate_WhenRateIsNegative_ShouldThrowArgumentException()
        {
            Action instantiateCurrency = () => ExchangeRate.Create(Currency.Create("ABC"), Currency.Create("DEF"), -1);

            instantiateCurrency.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ExchangeRate_WhenRateIsZero_ShouldThrowArgumentException()
        {
            Action instantiateCurrency = () => ExchangeRate.Create(Currency.Create("ABC"), Currency.Create("DEF"), 0);

            instantiateCurrency.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ExchangeRate_WhenRateAndCurrenciesAreValid_ShouldCreateAnInstance()
        {
            var exchangeRate = ExchangeRate.Create(Currency.Create("ABC"), Currency.Create("DEF"), 2);

            exchangeRate.Should().NotBeNull();
            exchangeRate.SourceCurrency.Code.Should().Be("ABC");
            exchangeRate.TargetCurrency.Code.Should().Be("DEF");
            exchangeRate.Value.Should().Be(2);
        }

    }
}
