using ExchangeRateUpdater.Domain.Exceptions;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;

namespace ExchangeRateUpdater.UnitTest.Domain.ValueObjects
{
    public class CurrencyTests
    {
        private readonly string _currencyISOCode = "EUR";
        private readonly string _currencyWrongISOCode = "EURO";

        [Fact]
        public void Create_Currency_Works()
        {
            // Act
            var currency = new Currency(_currencyISOCode);

            // Assert
            currency.Code.Should().Be(_currencyISOCode);
        }

        [Fact]
        public void Create_Currency_Throws_If_IncorrectISOCode()
        {
            // Act
            var act = () => new Currency(_currencyWrongISOCode);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage(Currency.InvalidIsoCodeMsg);
        }

        [Fact]
        public void Create_Currency_Throws_If_EmptyISOCode()
        {
            // Act
            var act = () => new Currency(string.Empty);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage(Currency.IsoCodeRequiredMsg);
        }
    }
}
