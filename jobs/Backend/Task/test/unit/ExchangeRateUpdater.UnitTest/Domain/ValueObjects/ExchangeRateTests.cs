using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;

namespace ExchangeRateUpdater.UnitTest.Domain.ValueObjects
{
    public class ExchangeRateTests
    {
        private readonly Currency _sourceCurrency;
        private readonly Currency _targetCurrency;
        private readonly decimal _value;

        public ExchangeRateTests()
        {
            _sourceCurrency = new Currency("AUD");
            _targetCurrency = new Currency("CZK");
            _value = 15.211M;
        }

        [Fact]
        public void Create_ExchangeRate_Works()
        {
            // Arrange and Act
            var exRate = new ExchangeRate(_sourceCurrency, _targetCurrency, _value);

            // Assert
            exRate.SourceCurrency.Should().Be(_sourceCurrency);
            exRate.TargetCurrency.Should().Be(_targetCurrency);
            exRate.Value.Should().Be(_value);
        }
    }
}
