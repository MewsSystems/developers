using ExchangeRateUpdater.Model;
using Assert = NUnit.Framework.Assert;

namespace ExchangeRateUpdater.Test
{
    public class ExchangeRateCacheTests
    {
        private readonly Currency USD = new("USD");
        private readonly Currency GBP = new("GBP");

        [Test]
        public void Clear()
        {
            // insert value
            ExchangeRateCache.Clear();
            ExchangeRateCache.GetCache()["key"] = new ExchangeRate(USD, GBP, 1.3m);

            // check value is present
            Assert.That(ExchangeRateCache.GetExchangeRateAsync("key"), Is.Not.EqualTo(null));

            // clear & check value has been cleared
            ExchangeRateCache.Clear();
            Assert.That(ExchangeRateCache.GetCache(), Is.Empty);
        }

        [Test]
        public void GetExchangeRateAsync()
        {
            ExchangeRateCache.Clear();
            // insert value
            var fxRate = new ExchangeRate(USD, GBP, 1.3m);
            ExchangeRateCache.GetCache()["key"] = fxRate;

            // check value is present & correct
            var result = ExchangeRateCache.GetExchangeRateAsync("key").Result;
            Assert.That(result, Is.EqualTo(fxRate));
        }
    }
}