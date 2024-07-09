using FluentAssertions;
using Xunit;
using System;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateTests
    {
        [Fact]
        public void Constructor_ShouldInitializeExchangeRate_WhenParametersAreValid()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 0.85m;

            // Act
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            exchangeRate.SourceCurrency.Should().Be(sourceCurrency);
            exchangeRate.TargetCurrency.Should().Be(targetCurrency);
            exchangeRate.Value.Should().Be(value);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSourceCurrencyIsNull()
        {
            // Arrange
            Currency sourceCurrency = null;
            var targetCurrency = new Currency("EUR");
            var value = 0.85m;

            // Act
            Action act = () => new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Source currency cannot be null. (Parameter 'sourceCurrency')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenTargetCurrencyIsNull()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            Currency targetCurrency = null;
            var value = 0.85m;

            // Act
            Action act = () => new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Target currency cannot be null. (Parameter 'targetCurrency')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenValueIsZero()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 0m;

            // Act
            Action act = () => new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Exchange rate value must be greater than zero. (Parameter 'value')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenValueIsNegative()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = -1m;

            // Act
            Action act = () => new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Exchange rate value must be greater than zero. (Parameter 'value')");
        }

        [Fact]
        public void ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 0.85m;
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Act
            var result = exchangeRate.ToString();

            // Assert
            result.Should().Be("USD/EUR=0.85");
        }
    }
}