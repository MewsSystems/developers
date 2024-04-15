using ExchangeRateUpdater;
using ExchangeRateUpdater.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateTests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {     
        private static IEnumerable<Currency> currencies = new[]  {
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


        [TestMethod]
        public async Task TestWebParser()
        {
            //Arrange
            string curDir = Directory.GetCurrentDirectory();           
            var testWeb = new HtmlWeb();
            HtmlNode mockData = testWeb.Load($"{curDir}\\mockData.html").DocumentNode;

            var mockExchangeRateProvider = new Mock<ExchangeRateProvider>(new Mock<ILogger<ExchangeRateProvider>>().Object, new Mock<IHttpClientFactory>().Object);

            mockExchangeRateProvider.Setup(s => s.LoadWeb(It.IsAny<CancellationToken>())).Returns(Task.FromResult(mockData));
            mockExchangeRateProvider.Setup(s => s.LoadApi(It.IsAny<CancellationToken>())).Returns(Task.FromResult(string.Empty));


            //Act
            var result = await mockExchangeRateProvider.Object.GetExchangeRates(currencies, new CancellationToken());            
            
            //Assert
            Assert.IsTrue(result.Count() == 5);
            Assert.IsTrue(currencies.Select(x => x.Code)
                          .Intersect(result.Select(s => s.SourceCurrency.Code))
                          .Any());
            
            Assert.IsFalse(result.Any(a=>a.SourceCurrency.Code.Equals("XYZ")));

            mockExchangeRateProvider.Verify(m => m.LoadApi(It.IsAny<CancellationToken>()), Times.Once);
            mockExchangeRateProvider.Verify(m => m.LoadWeb(It.IsAny<CancellationToken>()), Times.Once);
        }


        [TestMethod]
        public async Task TestApiParser()
        {
            //Arrange
            string curDir = Directory.GetCurrentDirectory();
            var json = File.ReadAllText($"{curDir}\\testData.json");         

            var mockExchangeRateProvider = new Mock<ExchangeRateProvider>(new Mock<ILogger<ExchangeRateProvider>>().Object, new Mock<IHttpClientFactory>().Object);

            mockExchangeRateProvider.Setup(s => s.LoadWeb(It.IsAny<CancellationToken>())).Returns(Task.FromResult(It.IsAny<HtmlNode>()));
            mockExchangeRateProvider.Setup(s => s.LoadApi(It.IsAny<CancellationToken>())).Returns(Task.FromResult(json));

            //Act
            var result = await mockExchangeRateProvider.Object.GetExchangeRates(currencies, It.IsAny<CancellationToken>());

            //Assert
            Assert.IsTrue(result.Count() == 5);
            Assert.IsTrue(currencies.Select(x => x.Code)
                          .Intersect(result.Select(s => s.SourceCurrency.Code))
                          .Any());

            Assert.IsFalse(result.Any(a => a.SourceCurrency.Code.Equals("XYZ")));

            mockExchangeRateProvider.Verify(m => m.LoadApi(It.IsAny<CancellationToken>()), Times.Once);
            mockExchangeRateProvider.Verify(m => m.LoadWeb(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}