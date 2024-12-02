using FluentAssertions;
using Xunit;
using System;

namespace ExchangeRateUpdater.Tests
{
    public class CurrencyTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCurrency_WhenCodeIsValid()
        {
            // Arrange
            var validCode = "USD";

            // Act
            var currency = new Currency(validCode);

            // Assert
            currency.Code.Should().Be(validCode);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsNull()
        {
            // Act
            Action act = () => new Currency(null);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code cannot be null or empty. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsEmpty()
        {
            // Act
            Action act = () => new Currency(string.Empty);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code cannot be null or empty. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsWhitespace()
        {
            // Act
            Action act = () => new Currency("   ");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code cannot be null or empty. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsTooShort()
        {
            // Act
            Action act = () => new Currency("US");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code must be a three-letter ISO 4217 code consisting of uppercase letters. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsTooLong()
        {
            // Act
            Action act = () => new Currency("USDE");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code must be a three-letter ISO 4217 code consisting of uppercase letters. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsLowercase()
        {
            // Act
            Action act = () => new Currency("usd");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code must be a three-letter ISO 4217 code consisting of uppercase letters. (Parameter 'code')");
        }

        [Fact]
        public void ToString_ShouldReturnCode()
        {
            // Arrange
            var validCode = "USD";
            var currency = new Currency(validCode);

            // Act
            var result = currency.ToString();

            // Assert
            result.Should().Be(validCode);
        }
    }
}

