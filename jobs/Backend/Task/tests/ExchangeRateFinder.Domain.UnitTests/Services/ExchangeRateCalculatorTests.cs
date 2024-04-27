using AutoFixture.Xunit2;
using ExchangeRateFinder.Domain.Services;

namespace ExchangeRateFinder.Domain.UnitTests.Services
{
    public class ExchangeRateCalculatorTests
    {
        private readonly ExchangeRateCalculator _target;

        public ExchangeRateCalculatorTests()
        {
            _target = new ExchangeRateCalculator();
        }

        [Theory]
        [InlineAutoData(1, 2.5, "USD", "EUR", 2.5f)]
        [InlineAutoData(10, 2, "GBP", "USD", 5)]
        [InlineAutoData(1000, 2.8, "GBP", "USD", 357.143f)]
        public void Calculate_ReturnsCorrectValues(
               int amount,
               decimal value,
               string sourceCurrencyCode,
               string targetCurrencyCode,
               string expected)
        {
            // Act
            var result = _target.Calculate(amount, value, sourceCurrencyCode, targetCurrencyCode);

            // Assert
            Assert.Equal(sourceCurrencyCode, result.SourceCurrencyCode);
            Assert.Equal(targetCurrencyCode, result.TargetCurrencyCode);
            Assert.Equal(expected, result.Rate.ToString("0.###"));
        }
    }
}