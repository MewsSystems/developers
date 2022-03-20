using ExchangeRateUpdater.helpers;
using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public void GetExchangeRates_ReturnsRatesForGivenCurrencies()
        {

            //setup
            var loggerMock = new Mock<ILogger>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            var httpClientMock = new Mock<HttpClient>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            var providedRates = new List<ExchangeRate>
            {
                {new ExchangeRate(new Currency("CZK"), new Currency("USD"), 1)},
                {new ExchangeRate(new Currency("CZK"), new Currency("JPY"), 2)},
                {new ExchangeRate(new Currency("CZK"), new Currency("RUB"), 3)}
            };

            var cnbProviderMock = new Mock<IRateProvider>();
            cnbProviderMock
                .Setup(s => s.GetExchangeRates())
                .ReturnsAsync(providedRates);

            var rateProviders = new Dictionary<string, IRateProvider>() { { "CNB", cnbProviderMock.Object } };

            var providerFactoryMock = new Mock<IExchangeRateProviderFactory>();
            providerFactoryMock
                .Setup(s => s.GetRateProviders())
                .Returns(rateProviders);

            ExchangeRateProvider provider = new ExchangeRateProvider(httpClientMock.Object, loggerFactoryMock.Object, providerFactoryMock.Object, dateTimeProviderMock.Object, loggerMock.Object);

          var currecies = new List<Currency>
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

            //act           
            var rates = provider.GetExchangeRates(currecies).ToList();

            Assert.Equal(3, rates.Count);
        }
    }
}
