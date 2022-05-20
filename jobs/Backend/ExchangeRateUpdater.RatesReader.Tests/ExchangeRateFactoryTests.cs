namespace ExchangeRateUpdater.RatesReader.Tests
{
    public class ExchangeRateFactoryTests
    {
        [Fact]
        public void GivenExchangeRateReadDataIsCorrent_WhenCalingExchangeRateFactory_ShouldSuccefullyCreateDomainObject()
        {
            var readModel = new ExchangeRateReadModel() { Amount = 1, CurrencyCode = "GBP", Rate = 10m };
            var domainModelResult = ExchangeRateFactory.CreateExchangeRateFromCZK(readModel);
            Assert.True(domainModelResult.Succsess);
            Assert.Equal(domainModelResult.Value.TargetCurrency, readModel.CurrencyCode);
            Assert.Equal(domainModelResult.Value.Value, readModel.Rate / readModel.Amount);
        }
        [Fact]
        public void GivenAmmountIsDifferentFromZero_WhenCreatingTheDomainObject_ShouldCorrectlyCreateTheExchangeRate()
        {
            var readModel = new ExchangeRateReadModel() { Amount = 100, CurrencyCode = "GBP", Rate = 10m };
            var domainModelResult = ExchangeRateFactory.CreateExchangeRateFromCZK(readModel);
            Assert.Equal(domainModelResult.Value.Value, readModel.Rate / readModel.Amount);
        }

        [Fact]
        public void GivenAmmountIsSmallerThanOne_WhenCalingExchangeRateFactory_ShouldNotCreateTheDomainObject()
        {
            var readModel = new ExchangeRateReadModel() { Amount = -10, CurrencyCode = "GBP", Rate = 10m };
            var domainModelResult = ExchangeRateFactory.CreateExchangeRateFromCZK(readModel);
            Assert.False(domainModelResult.Succsess);
        }

        [Fact]
        public void GivenCurrencyCodeIsIncorect_WhenCalingExchangeRateFactory_ShouldNotCreateTheDomainObject()
        {
            var readModel = new ExchangeRateReadModel() { Amount = 1, CurrencyCode = "GBPER", Rate = 10m };
            var domainModelResult = ExchangeRateFactory.CreateExchangeRateFromCZK(readModel);
            Assert.False(domainModelResult.Succsess);
        }

        [Fact]
        public void GivenCurrencyRateValueIsIncorect_WhenCalingExchangeRateFactory_ShouldNotCreateTheDomainObject()
        {
            var readModel = new ExchangeRateReadModel() { Amount = 0, CurrencyCode = "GBP"};
            var domainModelResult = ExchangeRateFactory.CreateExchangeRateFromCZK(readModel);
            Assert.False(domainModelResult.Succsess);
        }
    }
}
