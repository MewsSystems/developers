using ExchangeRateUpdater;
using ExchangeRateUpdater.Communication;
using ExchangeRateUpdater.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdate.UnitTests.Parsers
{
    public class ExchangeRateProviderTests
    {
        [Theory]
        [InlineData("USD", "EUR")]
        [InlineData("USD", "BGN")]
        [InlineData("CZK")]
        public async Task ProvideFilteredRates_WhenExpectedCurrenciesAreSet_ReturnsRatesAsync(params string[] currencies)
        {
            var config = new Mock<IExchangeRateConfiguration>();
            config.Setup(conf => conf.Url);
            var parser = new Mock<IParser<string, ExchangeRate>>();
            IEnumerable<ExchangeRate> parsedRes = new List<ExchangeRate> 
            { 
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 100),
                new ExchangeRate(new Currency("BGN"), new Currency("CZK"), 12)
            };
            parser.Setup(par => par.TryParse(It.IsAny<string>(), out parsedRes))
                .Returns(true);
            var httpClient = new Mock<IHttpsClientAdapter>();
            httpClient.Setup(client => client.GetAsync(It.IsAny<string>()));
            var provider = new ExchangeRateProvider(
                parser.Object,
                config.Object,
                httpClient.Object);

            var rates = await provider.GetExchangeRates(currencies.Select(code => new Currency(code)));

            Assert.NotNull(rates);
            Assert.True(rates.Count() == parsedRes.Where(rate => currencies.Any(cur => cur == rate.SourceCurrency.Code)).Count());
        }
    }
}
