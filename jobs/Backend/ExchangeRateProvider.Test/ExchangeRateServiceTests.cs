using ExchangeRateProvider.Cache;
using ExchangeRateProvider.Service;
using Model.Entities;
using NSubstitute;

namespace ExchangeRateProvider.Test
{
    [TestClass]
    public class ExchangeRateServiceTests
    {
        private readonly ICache<Currency, ExchangeRate> _exchangeRateCache = Substitute.For<ICache<Currency, ExchangeRate>>();
        private ExchangeRateService _exchangeRateService;
        private List<Currency> _currencies = new();
        private Dictionary<Currency, ExchangeRate> _cachedRates = new();

        [TestInitialize]
        public void Initialize()
        {
            _exchangeRateService = new ExchangeRateService(_exchangeRateCache);
            _exchangeRateCache.Index.Returns(_cachedRates);
        }


        [TestMethod]
        public void GetExchangeRates_Should_SameCurrencyWhenThereIsMatchBetweenCurrencies()
        {
            AddCurrencyToList("EUR");
            AddExchangeRateToCache("EUR", "CZK", 10);

            var rates = _exchangeRateService.GetExchangeRates(_currencies);

            Assert.AreEqual(new Currency("EUR"), rates.First().SourceCurrency);
        }

        [TestMethod]
        public void GetExchangeRates_Should_ReturnEmptyWhenCacheIsEmptyAndParametersAreEmpty()
        {
            var rates = _exchangeRateService.GetExchangeRates(_currencies);

            Assert.AreEqual(0, rates.Count());
        }

        [TestMethod]
        public void GetExchangeRates_Should_ReturnEmptyWhenCacheIsEmptyAndParametersAreNotEmpty()
        {
            AddCurrencyToList("EUR");

            var rates = _exchangeRateService.GetExchangeRates(_currencies);

            Assert.AreEqual(0, rates.Count());
        }

        [TestMethod]
        public void GetExchangeRates_Should_ReturnEmptyWhenCacheIsNotEmptyAndParametersAreEmpty()
        {
            AddExchangeRateToCache("EUR", "CZK", 10);

            var rates = _exchangeRateService.GetExchangeRates(_currencies);

            Assert.AreEqual(0, rates.Count());
        }

        [TestMethod]
        public void GetExchangeRates_Should_ReturnEmptyWhenCacheAndParametersAreDifferent()
        {
            AddCurrencyToList("USD");
            AddExchangeRateToCache("EUR", "CZK", 10);

            var rates = _exchangeRateService.GetExchangeRates(_currencies);

            Assert.AreEqual(0, rates.Count());
        }


        private void AddCurrencyToList(string sourceCode)
        {
            _currencies.Add(new Currency(sourceCode));
        }
        private void AddExchangeRateToCache(string sourceCode, string targetCode, decimal value)
        {
            var sourceCurrency = new Currency(sourceCode);
            var targetCurrency = new Currency(targetCode);

            _cachedRates.Add(sourceCurrency, new ExchangeRate(sourceCurrency, targetCurrency, value));
        }
    }
}