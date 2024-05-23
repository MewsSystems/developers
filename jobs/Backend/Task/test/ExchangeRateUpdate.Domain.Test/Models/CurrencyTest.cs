using ExchangeRateUpdater.Domain.DomainExceptions;
using ExchangeRateUpdater.Domain.Entities;
using FluentAssertions;

namespace ExchangeRateUpdate.Domain.Test.Models
{
    public class CurrencyTest
    {
        [Fact]
        public void Currency_WhenCurrencyCodeIsNull_ShouldThrowInvalidCurrencyFormatException()
        {
            Action instantiateCurrency = () => Currency.Create(null);

            instantiateCurrency.Should().Throw<InvalidCurrencyFormatException>();
        }

        [Fact]
        public void Currency_WhenCurrencyCodeIsEmpty_ShouldThrowInvalidCurrencyFormatException()
        {
            Action instantiateCurrency = () => Currency.Create(string.Empty);

            instantiateCurrency.Should().Throw<InvalidCurrencyFormatException>();
        }

        [Fact]
        public void Currency_WhenCurrencyCodeLenghtIsDifferentThan3_ShouldThrowInvalidCurrencyFormatException()
        {
            Action instantiateCurrency = () => Currency.Create("a");

            instantiateCurrency.Should().Throw<InvalidCurrencyFormatException>();
        }

        [Fact]
        public void Currency_WhenCurrencyCodeLenghtIsEquals3_ShouldCreateAnInstance()
        {
            var currency = Currency.Create("CZN");

            currency.Code.Should().Be("CZN");
        }
    }
}
