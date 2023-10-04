using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Test.Entitites
{
    public class ExchangeRateValueTests
    {

        [Fact]
        public void ExchangeRateValue_New_InputNegative()
        {
            Assert.Throws<ExchangeRateValueMustBePositiveException>(() => new ExchangeRateValue(-1));
        }

        [Fact]
        public void ExchangeRateValue_New_InputZero()
        {
            Assert.Throws<ExchangeRateValueMustBePositiveException>(() => new ExchangeRateValue(0));
        }

        [Fact]
        public void ExchangeRateValue_New_GreatherThanZero()
        {
            var exchangeRate = new ExchangeRateValue((decimal) 0.01);
            Assert.True(exchangeRate.Value == (decimal)0.01);
        }

    }
}
