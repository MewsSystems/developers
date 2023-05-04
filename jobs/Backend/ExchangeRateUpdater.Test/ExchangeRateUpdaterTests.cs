using Moq;
using System.Linq.Expressions;

namespace ExchangeRateUpdater.Test
{
    public class Tests
    {
        IEnumerable<Currency> currencies = new[]
       {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };

        [Test]
        public async Task GetDataTest()
        {
            //Arrange

            var dataRepository = new Mock<DataRepository>();

            var currencyCodes = currencies.Select(x => x.Code).ToList();

            dataRepository.Setup(x => x.GetData(It.IsAny<List<string>>(),It.IsAny<string>())).Returns(GetMockData());


            //Act

            var dataService = new DataService(dataRepository.Object);

            var result = await dataService.GetExchangeRateData(currencies);
            

            //Assert

            Assert.AreEqual(result.Count, 4);
        }

        [Test]
        public async Task GetExchangeRates_Returns_4()
        {
            //Arrange

            var dataRepository = new Mock<DataRepository>();

            var currencyCodes = currencies.Select(x => x.Code).ToList();

            dataRepository.Setup(x => x.GetData(It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(GetMockData());

            var dataService = new Mock<DataService>(dataRepository.Object);

            dataService.Setup(x => x.GetExchangeRateData(It.IsAny<IEnumerable<Currency>>()))
                .Returns(GetMockExchangeRateData());


            //Act

            var exchangeRateProvider = new ExchangeRateProvider(dataService.Object);

            var result = await exchangeRateProvider.GetExchangeRates(currencies);

            var resultList = result.ToList();

            //Assert

            Assert.AreEqual(resultList.Count, 4);

            Assert.AreEqual(resultList[0].SourceCurrency.Code, "USD");
            Assert.AreEqual(resultList[0].TargetCurrency.Code, "CZK");
            Assert.AreEqual(resultList[0].Value,10.00);

            Assert.AreEqual(resultList[1].SourceCurrency.Code, "GBP");
            Assert.AreEqual(resultList[0].TargetCurrency.Code, "CZK");
            Assert.AreEqual(resultList[1].Value, 10.00);

            Assert.AreEqual(resultList[2].SourceCurrency.Code, "AUD");
            Assert.AreEqual(resultList[2].TargetCurrency.Code, "CZK");
            Assert.AreEqual(resultList[2].Value, 14.00);

            Assert.AreEqual(resultList.Count, 4);
            Assert.AreEqual(resultList[3].SourceCurrency.Code, "EUR");
            Assert.AreEqual(resultList[3].TargetCurrency.Code, "CZK");
            Assert.AreEqual(resultList[3].Value, 0.2);

        }

        private static Task<List<ExchangeRateModel>> GetMockData()
        {
            var data =  new List<ExchangeRateModel> {

                new ExchangeRateModel
                {
                    Country = "USA",
                    Currency = "Dollar",
                    CurrencyCode = "USD",
                    Amount = "1",
                    Rate = "10.00"
                },
                new ExchangeRateModel
                {
                    Country = "UK",
                    Currency = "Pound",
                    CurrencyCode = "GBP",
                    Amount = "1",
                    Rate = "10.00"
                }

            };

            return Task.FromResult(data);
        }

        private static Task<List<ExchangeRateModel>> GetMockExchangeRateData()
        {
            var data = new List<ExchangeRateModel> {

                new ExchangeRateModel
                {
                    Country = "USA",
                    Currency = "Dollar",
                    CurrencyCode = "USD",
                    Amount = "1",
                    Rate = "10.00"
                },
                new ExchangeRateModel
                {
                    Country = "UK",
                    Currency = "Pound",
                    CurrencyCode = "GBP",
                    Amount = "1",
                    Rate = "10.00"
                },
                new ExchangeRateModel
                {
                    Country = "Australia",
                    Currency = "dollar",
                    CurrencyCode = "AUD",
                    Amount = "1",
                    Rate = "14.00"
                },
                new ExchangeRateModel
                {
                    Country = "EMU",
                    Currency = "euro",
                    CurrencyCode = "EUR",
                    Amount = "100",
                    Rate = "20.00"
                }
            };

            return Task.FromResult(data);
        }
    }
}


