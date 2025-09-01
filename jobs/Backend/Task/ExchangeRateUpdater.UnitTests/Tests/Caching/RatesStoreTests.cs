using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.Provider.Cnb.Dtos;
using ExchangeRateUpdater.UnitTests.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.UnitTests.Tests.Caching
{
    public class RatesStoreTests
    {
        [Fact]
        public void Get_ReturnsNull_WhenStoreIsEmpty()
        {
            //Arrange
            var store = TestData.CreateRatesStore();

            //Act
            var result = store.Get();

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void SetIfNewer_StoresRates_WhenEmpty()
        {
            // Arrange            
            var store = TestData.CreateRatesStore();

            var date = "2025-08-28";
            var first = TestData.CreateRatesResponse(
                date,
                new[]{ new CnbRate { CurrencyCode = "USD", Amount = 1, Rate = 12.3m } });

            // Act
            store.SetIfNewer(first);
            var got = store.Get();

            // Assert
            Assert.NotNull(got);
            Assert.Single(got.Rates);
            Assert.Equal(date, got.Rates[0].ValidFor);
        }

        [Fact]
        public void SetIfNewer_DoesNotUpdate_WhenCurrentIsNewer()
        {
            // Arrange
            var store = TestData.CreateRatesStore();

            var newerDate = "2025-08-28";
            var newer = TestData.CreateRatesResponse(newerDate, new[]
                { new CnbRate { CurrencyCode = "USD", Amount = 1, Rate = 12.3m }});
            
            var olderDate = "2025-08-27";
            var older = TestData.CreateRatesResponse(olderDate, new[]
                { new CnbRate { CurrencyCode = "USD", Amount = 1, Rate = 12.4m }});

            store.SetIfNewer(newer);

            // Act
            store.SetIfNewer(older);
            var got = store.Get();

            // Assert
            Assert.NotNull(got);
            Assert.Equal(newerDate, got.Rates[0].ValidFor);
        }

        [Fact]
        public void SetIfNewer_DoesUpdate_WhenCurrentIsOlder()
        {
            // Arrange
            var store = TestData.CreateRatesStore();

            var newerDate = "2025-08-28";
            var newer = TestData.CreateRatesResponse(newerDate, new[]
                { new CnbRate { CurrencyCode = "USD", Amount = 1, Rate = 12.3m }});

            var olderDate = "2025-08-27";
            var older = TestData.CreateRatesResponse(olderDate, new[]
                { new CnbRate { CurrencyCode = "USD", Amount = 1, Rate = 12.4m }});

            store.SetIfNewer(older);

            // Act
            store.SetIfNewer(newer);
            var got = store.Get();

            // Assert
            Assert.NotNull(got);
            Assert.Equal(newerDate, got.Rates[0].ValidFor);
        }
    }
}
