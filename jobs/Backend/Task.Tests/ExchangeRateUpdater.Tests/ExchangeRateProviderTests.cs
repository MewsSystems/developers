using ExchangeRateUpdater.Cnb;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<IBankApi> _bankApiMock = new Mock<IBankApi>();
        private readonly ExchangeRateProvider _exchangeRateProvider;

        public ExchangeRateProviderTests()
        {
            _exchangeRateProvider = new ExchangeRateProvider(_bankApiMock.Object);
        }

        [Fact]
        public async Task ShouldProvideOnlyCurrenciesSpecifiedInList()
        {
            ArrangeApiRates();

            var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
            var result = await _exchangeRateProvider.GetExchangeRates(currencies);

            var expected = new List<ExchangeRate>()
            {
                ExchangeRate.FromCZK(new Currency("USD"), 2.0M),
                ExchangeRate.FromCZK(new Currency("EUR"), 1.0M),
            };
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task ShouldReturnEmptyListIfEmptyRequestedCurrencies()
        {
            ArrangeApiRates();

            var currencies = Enumerable.Empty<Currency>();

            var result = await _exchangeRateProvider.GetExchangeRates(currencies);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ShouldThrowIfExpectedCurrenciesAreNull()
        {
            ArrangeApiRates();

            IEnumerable<Currency> currencies = null!; // whoopsie
            var result = _exchangeRateProvider.GetExchangeRates(currencies);
            await Assert.ThrowsAsync<ArgumentNullException>(() => result);
        }

        private void ArrangeApiRates()
        {
            _bankApiMock.Setup(x => x.GetTodayExchangeRates()).ReturnsAsync(new List<CnbExchangeRate>()
            {
                new CnbExchangeRate()
                {
                    Amount = 1,
                    Rate = 2.0M,
                    CurrencyCode = "USD"
                },
                new CnbExchangeRate()
                {
                    Amount = 1,
                    Rate = 1.0M,
                    CurrencyCode = "EUR"
                },
                new CnbExchangeRate()
                {
                    Amount = 1,
                    Rate = 3.0M,
                    CurrencyCode = "AUD"
                },
            });
        }
    }
}
