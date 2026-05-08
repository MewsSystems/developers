using ExchangeRateUpdater.Infrastructure.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.CzechNationalBank.DTOs;

namespace ExchangeRateUpdater.Tests.Infrastructure.CzechNationalBank
{
    public class CnbExchangeRateMapperTests
    {
        [Fact]
        public void Map_ReturnsCorrectExchangeRates_NormalizesValues()
        {
            // Arrange
            var dtoResponse = new CnbExchangeRateResponse
            {
                Rates = new List<CnbRate>
                {
                    new CnbRate
                    {
                        ValidFor = "2025-03-18",
                        Amount = 100,
                        CurrencyCode = "JPY",
                        Rate = 15.43m
                    },
                    new CnbRate
                    {
                        ValidFor = "2025-03-18",
                        Amount = 1,
                        CurrencyCode = "USD",
                        Rate = 23.50m
                    }
                }
            };

            // Act
            var results = CnbExchangeRateMapper.Map(dtoResponse).ToList();

            // Assert
            Assert.Equal(2, results.Count);
            var jpyRate = results.FirstOrDefault(r => r.SourceCurrency.Code.Equals("JPY", StringComparison.OrdinalIgnoreCase));
            Assert.NotNull(jpyRate);
            Assert.Equal(0.1543m, jpyRate.Value);
            Assert.Equal("CZK", jpyRate.TargetCurrency.Code);

            var usdRate = results.FirstOrDefault(r => r.SourceCurrency.Code.Equals("USD", StringComparison.OrdinalIgnoreCase));
            Assert.NotNull(usdRate);
            Assert.Equal(23.50m, usdRate.Value);
            Assert.Equal("CZK", usdRate.TargetCurrency.Code);
        }

        [Fact]
        public void Map_EmptyRatesList_ReturnsEmptyList()
        {
            // Arrange
            var dtoResponse = new CnbExchangeRateResponse
            {
                Rates = new List<CnbRate>()
            };

            // Act
            var results = CnbExchangeRateMapper.Map(dtoResponse);

            // Assert
            Assert.Empty(results);
        }
    }
}
