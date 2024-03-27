using ExchangeRateProvider.BusinessLogic;
using ExchangeRateProvider.DomainEntities;
using ExchangeRateProvider.Persistence.IRepo;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq;

namespace ExchangeRateProvider.Test
{
    public class CurrencyPairRatesTest
    {
        private CurrencyPairRates _currencyPairRates;
        private Mock<ICurrencyExchangeRepo> _currencyExchangeRepo = new Mock<ICurrencyExchangeRepo>();
  
        [Fact]
        public void GetAllAsyncTest_FormatsJson_ReturnsListOfCurrencyPairs()
        {
            // Arrange 

            var currencyPairValue = "CZK/AUD=1";
            var totalNumbersOfCurrencyPairs = 3;

            var requesModel = new RequestModel()
            {
                DateTime = DateTime.Now,
                Language = "en"
            };

            var uri = "cnbapi/exrates/daily?date=2024-03-27&lang=EN";

            string jsonData = "{'rates':[{'validFor':'2019-05-17','order':94,'country':'Australia','currency':'dollar','amount':1,'currencyCode':'AUD','rate':15.858},{'validFor':'2019-05-17','order':94,'country':'Brazil','currency':'real','amount':1,'currencyCode':'BRL','rate':5.686},{'validFor':'2019-05-17','order':94,'country':'Bulgaria','currency':'lev','amount':1,'currencyCode':'BGN','rate':13.162}]}";

            _currencyExchangeRepo
                .Setup(x => x.GetPairsAsync(uri))
                .ReturnsAsync(jsonData);

            var inMemorySettings = new Dictionary<string, string> {
                {"CzechNationalBankApiProperties:BaseAddress", "https://api.cnb.cz/"},
                {"CzechNationalBankApiProperties:DailyRatesApiPath", "cnbapi/exrates/daily"},
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Act
            _currencyPairRates = new CurrencyPairRates(
                _currencyExchangeRepo.Object, configuration);

            var result = _currencyPairRates.GetAllAsync(requesModel).Result;

            // Assert
            Assert.Equal(result.Rates.First(), currencyPairValue);
            Assert.Equal(result.Rates.Count(), totalNumbersOfCurrencyPairs);
        }
    }
}