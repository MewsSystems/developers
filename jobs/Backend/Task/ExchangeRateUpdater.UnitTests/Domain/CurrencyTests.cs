using ExchangeRateUpdater.Domain;
using Xunit;
using Assert = Xunit.Assert;

namespace ExchangeRateUpdater.UnitTests.Domain
{

    public class CurrencyTests
    {
        [Fact]
        public void Create_ShouldReturnCurrency_WhenIsOk()
        {
            var code = "EUR";

            var currency = Currency.Create(code);

            Assert.Equal(code, currency.Code);
        }

        [Fact]
        public void Compare_ShouldReturnEqual_WhenSameCode()
        {
            var c1 = Currency.Create("GBP");
            var c2 = Currency.Create("GBP");

            Assert.Equal(c1, c2);
        }

        [Fact]
        public void Compare_ShouldReturnNotEqual_WhenDifferentCode()
        {
            var c1 = Currency.Create("JPY");
            var c2 = Currency.Create("AUD");

            Assert.NotEqual(c1, c2);
        }
    }
}
