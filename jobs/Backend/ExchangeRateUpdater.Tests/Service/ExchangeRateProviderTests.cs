using ExchangeRateUpdater.Abstraction.Interfaces;
using ExchangeRateUpdater.Abstraction.Model;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace ExchangeRateUpdater.Tests.Service
{
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private IExchangeRateClient client = null!;
        private IMemoryCache cache = null!;

        [SetUp]
        public void SetUp()
        {
            client = Substitute.For<IExchangeRateClient>();
            cache = new MemoryCache(new MemoryCacheOptions());
        }

        [TearDown]
        public void TearDown()
        {
            cache.Dispose();
        }

        [Test]
        public async Task GetExchangeRatesAsync_FetchesFromClientAndCaches()
        {
            var now = DateTime.UtcNow;
            var usd = new ExchangeRate(new Currency("USD"), new Currency("CZK"), 25m, now);
            var eur = new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 27m, now);

            client.GetExchangeRatesAsync().Returns(Task.FromResult<IEnumerable<ExchangeRate>>(new[] { usd, eur }));

            var provider = new ExchangeRateProvider(client, NullLogger<ExchangeRateProvider>.Instance, cache);

            var requested = new[] { new Currency("usd") };

            var firstCall = await provider.GetExchangeRatesAsync(requested);
            Assert.That(firstCall, Is.Not.Null);
            var arr1 = firstCall.ToArray();
            Assert.That(arr1.Length, Is.EqualTo(1));
            Assert.That(arr1[0].SourceCurrency.Code, Is.EqualTo("USD"));

            // Client should have been called once
            await client.Received(1).GetExchangeRatesAsync();

            // Call again: should hit cache and not call client again
            var secondCall = await provider.GetExchangeRatesAsync(requested);
            await client.Received(1).GetExchangeRatesAsync();
            var arr2 = secondCall.ToArray();
            Assert.That(arr2.Length, Is.EqualTo(1));
            Assert.That(arr2[0].SourceCurrency.Code, Is.EqualTo("USD"));
        }

        [Test]
        public void GetExchangeRatesAsync_NullCurrencies_Throws()
        {
            var provider = new ExchangeRateProvider(client, NullLogger<ExchangeRateProvider>.Instance, cache);

            Assert.That(async () => await provider.GetExchangeRatesAsync(null!), Throws.TypeOf<ArgumentNullException>());
        }
    }
}
