using ExchangeRateUpdater.Abstractions.Contracts;
using ExchangeRateUpdater.Abstractions.Exceptions;
using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.Provider.Cnb.Dtos;
using ExchangeRateUpdater.Services.Providers;
using ExchangeRateUpdater.UnitTests.Shared;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.UnitTests.Tests.Services
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public void GetExchangeRates_ReturnsRequestedRates_WhenRatesExists()
        {
            // Arrange
            var validFor = "2025-08-28";
            var baseCurrency = "CZK";

            var usdCode = "USD";//requested and exists
            var usdUnits = 1;
            var usdRate = 12.3m;

            var phpCode = "PHP";//requested and exists
            var phpUnits = 1;
            var phpRate = 12.4m;

            var ignoredCode = "XYZ";//requested and does not exist

            var response = TestData.CreateRatesResponse(validFor, new[]
            {
                TestData.CreateRate(usdCode, usdUnits, usdRate, validFor),
                TestData.CreateRate(phpCode, phpUnits, phpRate, validFor),
                TestData.CreateRate("EUR", 1, 12.3m, validFor)//exists and its not requested
            });

            var provider = TestData.CreateProviderReturning(response);

            var requested = new[]
            {
                new Currency(usdCode),
                new Currency(phpCode),
                new Currency(ignoredCode)
            };

            // Act
            var result = provider.GetExchangeRates(requested).ToList();

            // Assert
            Assert.Equal(2, result.Count);

            var usd = result.Single(r => r.TargetCurrency.Code == usdCode);
            Assert.Equal(baseCurrency, usd.SourceCurrency.Code);
            Assert.Equal(usdRate, usd.Value);

            var php = result.Single(r => r.TargetCurrency.Code == phpCode);
            Assert.Equal(baseCurrency, php.SourceCurrency.Code);
            Assert.Equal(phpRate, php.Value);
        }

        [Fact]
        public void GetExchangeRates_ThrowsException_WhenStoreIsEmpty()
        {
            // Arrange
            var store = Substitute.For<IRatesStore>();
            store.Get().Returns(_ => null!);
            var provider = new ExchangeRateProvider(store);

            var requested = new[] { new Currency("USD") };

            // Act & Assert
            Assert.Throws<UnavailableRatesException>(() => 
                provider.GetExchangeRates(requested).ToList());
        }
    }
}
