using ExchangeRateUpdater.Domain;
using FluentAssertions;
using Xunit;

namespace ExchangeRateUpdater.UnitTests.Domain
{
    public class CurrencyTests
    {
        [Fact]
        public void Currency_DefaultCurrency_ShouldByCzechKoruna()
        {
            // Arrange + Act + Assert
            Currency.DEFAULT_CURRENCY.Code.Should().Be("CZK");
        }
    }
}
