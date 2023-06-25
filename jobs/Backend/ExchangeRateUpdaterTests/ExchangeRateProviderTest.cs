using AutoMapper;
using ExchangeRateUpdater;
using ToolsProvider.Helpers;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderTest
    {
        ExchangeRateProvider provider = new ExchangeRateProvider(MappingProvider.GetMapper());

        [Fact]
        public async void Check_If_GetDataFromSource_Not_Null()
        {

            var rates = await provider.GetExchangeRates();

            Assert.NotNull(rates);
        }

        [Fact]
        public async void Check_If_GetDataFromSource_Returns_Any_Data()
        {
            var rates = await provider.GetExchangeRates();

            Assert.NotEmpty(rates);
        }
    }
}