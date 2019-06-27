using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ExchangeRateUpdaterTests
{
    [TestClass]
    public class ExchangeRateProviderTest
    {
        [TestMethod]
        public void GetExchangeRates_From0626_Ok()
        {
            //Arrange
            Dictionary<string, decimal> parsed = new Dictionary<string, decimal>
            {
                { "AUD", 15.658M },
                { "BRL", 5.842M },
                { "BGN", 13.030M },
                { "CNY", 3.262M },
                { "DKK", 3.414M },
                { "EUR", 25.485M },
                { "PHP", 0.43598M },
                { "HKD", 2.873M },
                { "HRK", 3.446M },
                { "INR", 0.32440M },
                { "IDR", 0.001583M },
                { "ISK", 0.18011M },
                { "ILS", 6.245M },
                { "JPY", 0.20821M },
                { "ZAR", 1.565M },
                { "CAD", 17.054M },
                { "KRW", 0.01942M },
                { "HUF", 0.07877M },
                { "MYR", 5.409M },
                { "MXN", 1.169M },
                { "XDR", 31.219M },
                { "NOK", 2.634M },
                { "NZD", 14.989M },
                { "PLN", 5.978M },
                { "RON", 5.396M },
                { "RUB", 0.35567M },
                { "SGD", 16.563M },
                { "SEK", 2.416M },
                { "CHF", 22.941M },
                { "THB", 0.72932M },
                { "TRY", 3.894M },
                { "USD", 22.434M },
                { "GBP", 28.444M }

            };

            IEnumerable<Currency> input = new[]
            {
                new Currency("InvalidCode"),
                new Currency("SKK"),
                new Currency("GBP"),
                new Currency("EUR"),
                new Currency("HUF"),
                new Currency("InvalidCode"),
                new Currency("PLN"),
                new Currency("InvalidCode"),
            };

            List<ExchangeRate> expected = new List<ExchangeRate>(4)
            {
                new ExchangeRate(new Currency("GBP"),new Currency("CZK"),28.444M),
                new ExchangeRate(new Currency("EUR"),new Currency("CZK"),25.485M),
                new ExchangeRate(new Currency("HUF"),new Currency("CZK"),0.07877M),
                new ExchangeRate(new Currency("PLN"),new Currency("CZK"),5.978M)
            };

            //string expectedUrl = $"{Constants.CnbUrl}?date={date.Value:dd.MM.yyyy}";
            string expectedUrl = Constants.CnbUrl;

            Mock<IConnector> connectorMock = new Mock<IConnector>();
            connectorMock
                .Setup(m => m.DownloadTxtFile(expectedUrl))
                .Returns(Constants.Source1)
                .Verifiable();

            Mock<IParser> parserMock = new Mock<IParser>();
            parserMock
                .Setup(m => m.Parse(Constants.Source1))
                .Returns(parsed)
                .Verifiable();

            //the system under test
            ExchangeRateProvider provider = new ExchangeRateProvider(connectorMock.Object, parserMock.Object);

            //Act
            List<ExchangeRate> output = provider.GetExchangeRates(input).ToList();


            //Assert
            connectorMock.Verify();
            parserMock.Verify();

            int expectedCount = expected.Count;
            int outputCount = output.Count;
            Assert.AreEqual(expectedCount, outputCount);

            for (int i = 0; i < outputCount; i++)
            {
                ExchangeRate expObj = expected[i];
                ExchangeRate outputObj = output[i];
                Assert.AreEqual(expObj.SourceCurrency.Code, outputObj.SourceCurrency.Code);
                Assert.AreEqual(expObj.TargetCurrency.Code, outputObj.TargetCurrency.Code);
                Assert.AreEqual(expObj.Value, outputObj.Value);
                Assert.AreEqual(expObj.ToString(), outputObj.ToString());
            }
        }

    }
}
