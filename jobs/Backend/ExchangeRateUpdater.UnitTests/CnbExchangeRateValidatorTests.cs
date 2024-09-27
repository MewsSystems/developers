using ExchangeRateUpdater.Application;
using System.Collections.Generic;
using Xunit;

namespace ExchangeRateUpdater.UnitTests
{
    public class CnbExchangeRateValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_LessOrEqualToZeroAmount_ReturnsInvalidResult(long amount)
        {
            // Arrange
            var sut = new CnbExchangeRateValidator();
            var exchangeRate = new CnbExchangeRate(Amount: amount, CurrencyCode: string.Empty, Rate: 1.0m);

            // Act
            var validationResult = sut.Validate(exchangeRate);

            // Assert
            Assert.False(validationResult.IsValid);
        }

        public static IEnumerable<object[]> Data1 =>
            new List<object[]>
            {
                new object[] { 0m },
                new object[] { -1m },
            };

        [Theory, MemberData(nameof(Data1))]
        public void Validate_LessOrEqualToZeroRate_ReturnsInvalidResult(decimal rate)
        {
            // Arrange
            var sut = new CnbExchangeRateValidator();
            var exchangeRate = new CnbExchangeRate(Amount: 1, CurrencyCode: string.Empty, Rate: rate);

            // Act
            var validationResult = sut.Validate(exchangeRate);

            // Assert
            Assert.False(validationResult.IsValid);
        }

        public static IEnumerable<object[]> Data2 =>
            new List<object[]>
            {
                new object[] { 1, 1m }
            };

        [Theory, MemberData(nameof(Data2))]
        public void Validate_ValidInput_ReturnsValidResult(long amount, decimal rate)
        {
            // Arrange
            var sut = new CnbExchangeRateValidator();
            var exchangeRate = new CnbExchangeRate(Amount: amount, CurrencyCode: string.Empty, Rate: rate);

            // Act
            var validationResult = sut.Validate(exchangeRate);

            // Assert
            Assert.True(validationResult.IsValid);
        }
    }
}
