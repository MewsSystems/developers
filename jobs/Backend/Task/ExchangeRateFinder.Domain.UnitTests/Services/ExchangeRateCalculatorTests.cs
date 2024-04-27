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
               decimal rate,
               string sourceCurrency,
               string targetCurrency,
               string expected)
        {
            // Act
            var result = _target.Calculate(amount, rate, sourceCurrency, targetCurrency);

            // Assert
            Assert.Equal(sourceCurrency, result.SourceCurrency);
            Assert.Equal(targetCurrency, result.TargetCurrency);
            Assert.Equal(expected, result.Value.ToString("0.###"));
        }
    }
}