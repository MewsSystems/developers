using System.Threading.Tasks;
using ExchangeRateUpdater;
using NUnit.Framework;

namespace ExchangeRateUpdaterTests
{
    public class IntegrationTests
    {
        private readonly Currency[] _currencies = {new Currency("USD"), new Currency("CZK"), new Currency("EUR") };
        
        [Test]
        public async Task ExchangeRateProvider()
        {
            var target = new ExchangeRateProvider(new CnbClient(new CustomHttpClient()));
            var result = await target.GetExchangeRates(_currencies);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }
    }
}