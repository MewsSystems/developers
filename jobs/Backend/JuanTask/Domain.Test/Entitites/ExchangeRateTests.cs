using Domain.Entities;
using Domain.Exceptions;

namespace Domain.Test.Entitites
{
    public class ExchangeRateTests
    {

        [Fact]

        public void ExchangeRate_Create_ValidInpunts()
        {
            var exchangeRate = ExchangeRate.Create("ASE", "EUR", 1);
            Assert.True(exchangeRate.SourceCurrency.Code == "ASE");
            Assert.True(exchangeRate.TargetCurrency.Code == "EUR");
            Assert.True(exchangeRate.Value.Value == 1);
        }

        [Fact]

        public void ExchangeRate_Create_NegativeAmount()
        {
            Assert.Throws<ExchangeRateAmountMustBeGreaterThanZeroException>(() => ExchangeRate.Create("ASE", "EUR", -1, 0));
        }

        [Fact]

        public void ExchangeRate_Create_ZeroAmount()
        {
            Assert.Throws<ExchangeRateAmountMustBeGreaterThanZeroException>(() => ExchangeRate.Create("ASE", "EUR", 0, 1));
        }

        [Fact]

        public void ExchangeRate_Create_ValidAmount()
        {
            var exchangeRate = ExchangeRate.Create("ASE", "EUR", 100, 27);
            Assert.True(exchangeRate.SourceCurrency.Code == "ASE");
            Assert.True(exchangeRate.TargetCurrency.Code == "EUR");
            Assert.True(exchangeRate.Value.Value == (decimal)0.27);
        }

    }
}
