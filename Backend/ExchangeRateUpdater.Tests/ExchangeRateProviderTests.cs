using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        string defaultHeader =
            "14.06.2019 #114\n" +
            "země|měna|množství|kód|kurz\n";

        [Fact]
        public void GetExchangeRates_RatesNotRequestedAreNotReturned()
        {
            var requestedCurrencies = new List<Currency>
            {
                new Currency("AUD")
            };

            var mockClient = new Mock<CnbRateClient>();
            mockClient.Setup(x => x.GetRatesDataFromSource()).Returns("Velká Británie|libra|1|GBP|28,669");

            var sut = new ExchangeRateProvider(mockClient.Object);

            Assert.Empty(sut.GetExchangeRates(requestedCurrencies));
        }

        [Fact]
        public void GetExchangeRates_RequestedRatesAreReturned()
        {
            var requestedCurrencies = new List<Currency>
            {
                new Currency("AUD"),
                new Currency("GBP")
            };

            var mockClient = new Mock<CnbRateClient>();
            mockClient.Setup(x => x.GetRatesDataFromSource()).Returns(defaultHeader + "Velká Británie|libra|1|GBP|28,669\n");
            
            var sut = new ExchangeRateProvider(mockClient.Object);

            var result = sut.GetExchangeRates(requestedCurrencies);
            
            Assert.Single(result);
            Assert.Equal(new Currency("CZK"), result.FirstOrDefault().TargetCurrency);
            Assert.Equal(new Currency("GBP"), result.FirstOrDefault().SourceCurrency);
            Assert.Equal(new decimal(28.669), result.FirstOrDefault().Value);
        }

        [Fact]
        public void GetExchangeRates_BadlyFormattedDataDoesNotFailWholeJob()
        {
            var requestedCurrencies = new List<Currency>
            {
                new Currency("AUD"),
                new Currency("GBP")
            };

            var sourceData = defaultHeader +
                "BadDataFromSource\n" +
                "Velká Británie|libra|1|GBP|28,669\n";

            var mockClient = new Mock<CnbRateClient>();
            mockClient.Setup(x => x.GetRatesDataFromSource()).Returns(sourceData);

            var sut = new ExchangeRateProvider(mockClient.Object);

            var result = sut.GetExchangeRates(requestedCurrencies);

            Assert.Single(result);
            Assert.Equal("GBP", result.FirstOrDefault().SourceCurrency.Code);
        }
    }
}
