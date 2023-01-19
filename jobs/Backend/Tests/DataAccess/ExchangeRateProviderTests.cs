using DataAccess.Abstract;
using DataAccess.Concrete;
using Moq;
using Tests.Helpers;

namespace Tests.DataAccess
{
    public class ExchangeRateProviderTests
    {
        private Mock<IExchangeRateAccessor> _exchangeRateAccessor=new();

        public ExchangeRateProviderTests()
        {
            _exchangeRateAccessor.Setup(x => x.GetExchangeRates()).Returns(() => FakeDataHelper.CreateFakeExchangeRateList());
        }

        [Fact]
        public async Task GetExchangeRates_Success_WithCurrencies()
        {
            //arrange
            var _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateAccessor.Object);

            //act
            var _providerResponse =
                _exchangeRateProvider.GetExchangeRates(FakeDataHelper.CreateFakeCurrencyListRecord().Currencies);
            
            //assert           
            Assert.NotEmpty(_providerResponse);

        }

        [Fact]
        public async Task GetExchangeRates_Fails_WithMissingCurrencies()
        {
            //arrange
            _exchangeRateAccessor.Setup(x => x.GetExchangeRates()).Returns(() => FakeDataHelper.CreateFakeExchangeRateList());
            var _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateAccessor.Object);

            //act
            var _providerResponse =
                _exchangeRateProvider.GetExchangeRates(FakeDataHelper.CreateFakeWrongCurrencyListRecord().Currencies);

            //assert
            Assert.Empty(_providerResponse);

        }

    }
}
