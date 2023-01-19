using Business.Concrete;
using DataAccess.Abstract;
using Moq;
using Tests.Helpers;

namespace Tests.Business
{
    public class ExchangeRateProviderManagerTests
    {
        private Mock<IExchangeRateProvider> _exchangeRateProvider= new ();
        public ExchangeRateProviderManagerTests()
        {
            _exchangeRateProvider.Setup(x => x.GetExchangeRates(FakeDataHelper.CreateFakeCurrencyListRecord().Currencies)).Returns(() => FakeDataHelper.CreateFakeExchangeRateList());
        }

        [Fact]
        public async Task GetExchangeRates_Success_WithCurrencies()
        {
            //arrange
            var _exchangeRateProviderServiceManager = new ExchangeRateProviderManager(_exchangeRateProvider.Object);
            //act
            var serviceResponse = 
                _exchangeRateProviderServiceManager.GetExchangeRates(FakeDataHelper.CreateFakeCurrencyListRecord());
            //assert
            Assert.True(serviceResponse.Success);
           
        }

        [Fact]
        public async Task GetExchangeRates_Fails_WithMissingCurrencies()
        {
            //arrange
            var exchangeRatesProviderManager = new ExchangeRateProviderManager(_exchangeRateProvider.Object);
            //act
            var serviceResponse = exchangeRatesProviderManager.GetExchangeRates(FakeDataHelper.CreateFakeWrongCurrencyListRecord());
            //assert
            Assert.False(serviceResponse.Success);
            Assert.False(serviceResponse.Data!.Any());
        }
    }
}
