using ExchangeRateProvider.Contract.Models;

namespace ExchangeRateProvider.Contract.Tests.Models
{
    public class CurrencyTests
    {
        [Theory]
        [InlineData("CZK", "USD")]
        [InlineData("CZK", "CZK")]
        [InlineData("", "EUR")]
        [InlineData("CZK", "EUR")]
        public void ComparerWorks(string code1, string code2)
        {
            Currency c1 = new Currency(code1);
            Currency c2 = new Currency(code2);
            if(code1.Equals(code2))
                Assert.Equal(c1, c2);
            else
                Assert.NotEqual(c1, c2);
        }
    }
}