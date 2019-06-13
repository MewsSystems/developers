using Mews.Backend.Task.Core;
using Xunit;

namespace Mews.Backend.Task.UnitTests
{
    public class CzechBankRateProviderTests
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;

        public CzechBankRateProviderTests()
        {
            _exchangeRateProvider = new CzechBankRateProvider(new ExchangeRateParserFake());
        }

        [Fact]
        public async System.Threading.Tasks.Task GetExchangeRatesAsync_ReturnsAll()
        {
            var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(new[]
            {
                new Currency("TRY"),
                new Currency("GBP"),
                new Currency("USD")
            });

            Assert.Equal(3, exchangeRates.Count);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetExchangeRatesAsync_ReturnsSingle()
        {
            var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(new[]
            {
                new Currency("AAA"),
                new Currency("BBB"),
                new Currency("USD")
            });

            Assert.Single(exchangeRates);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetExchangeRates_ReturnsNothing()
        {
            var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(new[]
            {
                new Currency("AAA"),
                new Currency("BBB"),
                new Currency("CCC")
            });

            Assert.Empty(exchangeRates);
        }
    }
}
