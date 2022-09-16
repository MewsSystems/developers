using CzechNationalBankGateway;
using ExchangeRateProvider.Cache;
using ExchangeRateProvider.Service;
using Model.Entities;
using NSubstitute;
using System.Net;
using System.Security.Claims;

namespace ExchangeRateProvider.Test
{
    [TestClass]
    public class ExchangeRateCacheTests
    {
        private readonly IExchangeRateGatewayCNB _exchangeRateGatewayCNB = Substitute.For<IExchangeRateGatewayCNB>();
        private ExchangeRateCache _exchangeRateCache;
        private List<ExchangeRate> _rates = new();

        [TestInitialize]
        public void Initialize()
        {
            _exchangeRateCache = new ExchangeRateCache(_exchangeRateGatewayCNB);
            _exchangeRateGatewayCNB.GetExchangeRates().Returns(_rates);
        }

        [TestMethod]
        public void GetExchangeRates_Should_GetSameCurrencyWhenThereIsMatchBetweenCurrencies()
        {
            var rate = GetAndAddExchangeRateToList("EUR", "CZK", 10);

            var rates = _exchangeRateCache.Index;

            Assert.AreEqual(rate, rates[new Currency("EUR")]);
        }

        [TestMethod]
        public void ExchangeRatesCache_Should_NotQueryECNSecondTime()
        {
            var rate1 = GetAndAddExchangeRateToList("EUR", "CZK", 10);

            _ = _exchangeRateCache.Index;

            var rate2 = CreateExchangeRate("EUR", "CZK", 20);
            _rates.Clear();
            _rates.Add(rate2);

            var rates2 = _exchangeRateCache.Index;

            Assert.AreEqual(rate1, rates2[new Currency("EUR")]);
        }


        [TestMethod]
        public void ExchangeRatesCache_Should_QueryECNSecondTimeIfValiditySpanAllows()
        {
            _exchangeRateCache.SetValiditySpan(new TimeSpan(0, 0, 0));

            _ = GetAndAddExchangeRateToList("EUR", "CZK", 10);
            _ = _exchangeRateCache.Index;

            var rate2 = CreateExchangeRate("EUR", "CZK", 20);
            _rates.Clear();
            _rates.Add(rate2);

            var rates2 = _exchangeRateCache.Index;

            Assert.AreEqual(rate2, rates2[new Currency("EUR")]);
        }


        private ExchangeRate GetAndAddExchangeRateToList(string sourceCode, string targetCode, decimal value)
        {
            var rate = CreateExchangeRate(sourceCode, targetCode, value);
            _rates.Add(rate);

            return rate;
        }

        private ExchangeRate CreateExchangeRate(string sourceCode, string targetCode, decimal value)
        {
            var sourceCurrency = new Currency(sourceCode);
            var targetCurrency = new Currency(targetCode);
            return new ExchangeRate(sourceCurrency, targetCurrency, value);
        }
    }
}