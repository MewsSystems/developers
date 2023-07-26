using ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1;
using ExchangeRateUpdater.Models.Domain;

namespace ExchangeRateUpdater.Features.Tests.Services.ExchangeRatesDaily.V1
{
    public class MapperTests
    {
        [Fact]
        public void When_MapCurrencyModelList_Obtain_CurrencyList()
        {

            List<CurrencyModel> currencyModels = new List<CurrencyModel>
            {
                new CurrencyModel("CZK"),
                new CurrencyModel("AUS")
            };

            var actual = currencyModels.ToCurrency();

            Assert.Equal(currencyModels.Count(), actual.Count());

            var czkCurrency = currencyModels.First(x => x.Code == "CZK");
            Assert.Equal("CZK", czkCurrency.ToString());


            var ausCurrency = currencyModels.First(x => x.Code == "AUS");
            Assert.Equal("AUS", ausCurrency.ToString());
        }

        [Theory]
        [InlineData("CZK/AUS=1")]
        public void When_MapExchangeRatelList_Obtain_ExchangeModelList(
            string expectedEchangeRate)
        {
            List<ExchangeRate> exchangeRateList = new List<ExchangeRate>
            {
               new ExchangeRate( new Currency("CZK"), new Currency("AUS"), 1)
            };

            var actual = exchangeRateList.ToExchangeRateModel();

            Assert.Equal(exchangeRateList.Count(), actual.Count());

            var actualToString = exchangeRateList.First(x => x.TargetCurrency.Code == "AUS").ToString();
            Assert.Equal(expectedEchangeRate, actualToString);
        }
    }
}
