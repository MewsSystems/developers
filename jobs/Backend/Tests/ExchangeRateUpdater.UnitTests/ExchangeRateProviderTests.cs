using ExchangeRateUpdater.Fetch;
using ExchangeRateUpdater.Parse;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.UnitTests
{
    public class ExchangeRateProviderTests
    {
        private MockRepository mockRepository;
        private Mock<IExchangeRatesTxtFetcher> exchangeRatesTxtFetcherMock;
        private Mock<IExchangeRatesParser> exchangeRatesParser;
        private ExchangeRateProvider testableExchangeRateProvider;

        public ExchangeRateProviderTests()
        {
            mockRepository = new MockRepository(MockBehavior.Loose);
            exchangeRatesTxtFetcherMock = this.mockRepository.Create<IExchangeRatesTxtFetcher>();
            exchangeRatesParser = this.mockRepository.Create<IExchangeRatesParser>();
            testableExchangeRateProvider = new ExchangeRateProvider(exchangeRatesTxtFetcherMock.Object, exchangeRatesParser.Object);
        }


        [Fact]
        public void ShouldFilterExchangeRates()
        {
            exchangeRatesTxtFetcherMock.Setup(fetcher => fetcher.FetchExchangeRates()).ReturnsAsync("");
            exchangeRatesParser.Setup(parser => parser.ParseRates(It.IsAny<string>())).Returns(new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 20M),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25M)
            });
            var currencies = new List<Currency>
            {
                new Currency("USD")
            };

            var rates = testableExchangeRateProvider.GetExchangeRates(currencies);

            Assert.NotEmpty(rates);
            Assert.Single(rates);
            Assert.DoesNotContain(rates, rate => rate.SourceCurrency.Code == "EUR");
            Assert.Contains(rates, rate => rate.SourceCurrency.Code == "USD");
            mockRepository.VerifyAll();
        }

        [Fact]
        public void ShouldNotCrateNewExchangeRates()
        {
            exchangeRatesTxtFetcherMock.Setup(fetcher => fetcher.FetchExchangeRates()).ReturnsAsync("");
            exchangeRatesParser.Setup(parser => parser.ParseRates(It.IsAny<string>())).Returns(new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 20M),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25M)
            });
            var currencies = new List<Currency>
            {
                new Currency("EUR"), 
                new Currency("USD"), 
                new Currency("RUB")
            };

            var rates = testableExchangeRateProvider.GetExchangeRates(currencies);

            Assert.NotEmpty(rates);
            Assert.Equal(2, rates.Count());
            Assert.DoesNotContain(rates, rate => rate.SourceCurrency.Code == "RUB");
            Assert.Contains(rates, rate => rate.SourceCurrency.Code == "USD");
            mockRepository.VerifyAll();
        }
    }
}
