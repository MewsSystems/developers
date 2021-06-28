using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class CnbExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_GivenNullCurrencies_ReturnEmptyExchangeRates()
        {
            var provider = new CnbExchangeRateProvider(new HttpClient(), new Configuration());

            var rates = await provider.GetExchangeRatesAsync(null);
            
            Assert.Empty(rates);
        }
        
        [Fact]
        public async Task GetExchangeRatesAsync_GivenEmptyCurrencies_ReturnEmptyExchangeRates()
        {
            var provider = new CnbExchangeRateProvider(new HttpClient(), new Configuration());

            var rates = await provider.GetExchangeRatesAsync(Enumerable.Empty<Currency>());
            
            Assert.Empty(rates);
        }
    }
}