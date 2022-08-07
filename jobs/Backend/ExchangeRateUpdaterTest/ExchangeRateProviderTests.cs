using ExchangeRateUpdater;
using ExchangeRateUpdater.ExchangeRateDataProviders;
using ExchangeRateUpdater.ExchangeRateParsers;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdaterTest
{
    public class ExchangeRateProviderTests
    {
        private static readonly IEnumerable<Currency> currencies = new[]
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

        [Fact]
        public void GetExchangeRates_Success()
        {
            var data = new Mock<IExchangeRateDataSource>();
            var fs = new FileStream("TestData/GoodData.txt", FileMode.Open);
            data.Setup(x => x.GetDataAsync(default)).ReturnsAsync(fs);

            var parcer = new CnbExchangeRateParser();

            var provider = new ExchangeRateProvider(data.Object, parcer);
            var result = provider.GetExchangeRatesAsync(currencies).GetAwaiter().GetResult();

            Assert.NotNull(result);
            Assert.True(result.Count() == 5);
        }
    }
}
