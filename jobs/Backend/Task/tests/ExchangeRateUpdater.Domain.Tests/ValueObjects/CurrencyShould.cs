using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;

namespace ExchangeRateUpdater.Domain.Tests.ValueObjects
{
    public class CurrencyShould
    {
        [Fact]
        public void HaveUpperCaseCode()
        {
            var currency = new Currency("aud");

            currency.Code.Should().Be("AUD");
        }

        [Fact]
        public void BeEqualBasedOnCodeValue()
        {
            var aud1 = new Currency("aud");
            var aud2 = new Currency("AUD");

            var equality = aud1 == aud2;

            equality.Should().BeTrue();
        }
    }
}
