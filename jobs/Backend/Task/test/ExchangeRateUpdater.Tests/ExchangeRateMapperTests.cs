using ExchangeRateUpdater.Mappers;
using ExchangeRateUpdater.Models;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateMapperTests
    {

        [Fact]
        public void Map_ApiFxRate_To_ExchangeRate_()
        {
            var apiFxRates = new List<FxRate>
            {
                new() { CurrencyCode = "USD", Rate = 26.331 },
                new() { CurrencyCode = "GBP", Rate = 30.014 },
                new() { CurrencyCode = "RON", Rate = 8.769 },
                new() { CurrencyCode = "AUD", Rate = 15.164 }
            };

            var exchangeRates = ExchangeRateMapper.MapApiFxRatesToExchangeRates(apiFxRates).ToList();

            exchangeRates.Count.Should().Be(apiFxRates.Count);

            for (var i = 0; i < exchangeRates.Count; i++)
            {
                exchangeRates[i].SourceCurrency.Code.Should().Be(apiFxRates[i].CurrencyCode);
                exchangeRates[i].TargetCurrency.Code.Should().Be("CZK");
                exchangeRates[i].Value.Should().Be((decimal)apiFxRates[i].Rate);
            }
        }

        [Fact]
        public void MapApiFxRatesToExchangeRates_WithEmptyList_ShouldReturnEmpty()
        {
            var apiFxRates = new List<FxRate>();

            var exchangeRates = ExchangeRateMapper.MapApiFxRatesToExchangeRates(apiFxRates);

            exchangeRates.Should().BeEmpty();
        }

        [Fact]
        public void MapApiFxRatesToExchangeRates_ShouldHandleNullFxRateList()
        {
            var exchangeRates = ExchangeRateMapper.MapApiFxRatesToExchangeRates(null);

            exchangeRates.Should().BeEmpty();
        }
    }
}