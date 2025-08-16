using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.UnitTests.Helpers;
using FluentAssertions;
using System;
using Xunit;

namespace ExchangeRateUpdater.UnitTests.Domain
{
    public class ExchangeRateTests
    {
        [Fact]
        public void ToString_WithValidExchangeRate_ReturnsExpectedString()
        {
            // Arrange
            var czkToUsd = new ExchangeRate(
                sourceCurrency: CurrenciesHelper.CZK,
                targetCurrency: CurrenciesHelper.USD,
                value: 3.14m);

            // Act
            var rateString = czkToUsd.ToString();

            // Assert
            rateString.Should().Be("CZK/USD=3,14");
        }

        [Fact]
        public void Constructor_WithNullSourceCurrency_RaisesNullArgumentException()
        {
            // Arrange + Act
            var action = () => new ExchangeRate(
                sourceCurrency: null, 
                targetCurrency: CurrenciesHelper.CZK, 
                value: 1);

            // Assert
            action.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithNullTargetCurrency_RaisesNullArgumentException()
        {
            // Arrange + Act
            var action = () => new ExchangeRate(
                sourceCurrency: CurrenciesHelper.CZK,
                targetCurrency: null,
                value: 1);

            // Assert
            action.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithSameSourceAndTargetCurrency_RaisesApplicationException()
        {
            // Arrange + Act
            var action = () => new ExchangeRate(
                sourceCurrency: CurrenciesHelper.CZK,
                targetCurrency: CurrenciesHelper.CZK,
                value: 1);

            // Assert
            action.Should()
                .Throw<ApplicationException>();
        }

        [Fact]
        public void Constructor_WithNegativeValue_RaisesApplicationException()
        {
            // Arrange + Act
            var action = () => new ExchangeRate(
                sourceCurrency: CurrenciesHelper.CZK,
                targetCurrency: CurrenciesHelper.USD,
                value: -0.1m);

            // Assert
            action.Should()
                .Throw<ApplicationException>();
        }
    }
}
