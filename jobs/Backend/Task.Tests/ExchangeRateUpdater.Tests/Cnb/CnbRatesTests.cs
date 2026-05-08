using ExchangeRateUpdater.Cnb;

namespace ExchangeRateUpdater.Tests.Cnb
{
    public class CnbRatesTests
    {

        [Theory]
        [InlineData(1, 32.2, 32.2)]
        [InlineData(100, 32.2, 0.322)]
        [InlineData(50, 32.2, 0.644)]
        public void ShouldCalculateRealAmount(int amount, decimal rate, decimal expectedAmount)
        {
            var cnbRate = new CnbExchangeRate()
            {
                Amount = amount,
                Rate = rate
            };
            Assert.Equal(expectedAmount, cnbRate.RealRate);
        }
    }
}
