using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class ExchangeRateProviderTests
    {
        private readonly IExchangeRateClient exchangeClient;
        private readonly ExchangeRateProvider exchangeRateProvider;

        public ExchangeRateProviderTests()
        {
            exchangeClient = Substitute.For<IExchangeRateClient>();
            exchangeRateProvider = new ExchangeRateProvider(exchangeClient);
        }

        [TestMethod]
        public async Task GetExchangeRates_WithNoCurrencies_ReturnsNoExchanges()
        {
            var empty = Enumerable.Empty<Currency>();

            var exhangeRates = await exchangeRateProvider
                .GetExchangeRates(empty)
                .ToListAsync();

            exchangeClient.GetExchanges(Arg.Any<IEnumerable<Currency>>()).DidNotReceive();
            Assert.IsTrue(exhangeRates.Count == 0);
        }

        [TestMethod]
        public async Task GetExchangeRates_WithNoCurrencies_ReturnsCalculatedRateExchanges()
        {
            async IAsyncEnumerable<(int amout, string code, decimal rate)> GetExchanges()
            {
                yield return (2, "USD", 20);
            }
            var currencies = new List<Currency> { new Currency("USD") };
            exchangeClient.GetExchanges(currencies)
                .Returns(GetExchanges());

            var exhangeRates = await exchangeRateProvider
                .GetExchangeRates(currencies)
                .ToListAsync();

            Assert.IsTrue(exhangeRates.Count == 1);
            Assert.IsTrue(exhangeRates[0].SourceCurrency.Code == "USD");
            Assert.IsTrue(exhangeRates[0].TargetCurrency.Code == "CZK");
            Assert.IsTrue(exhangeRates[0].Value == 10);
        }
    }
}
