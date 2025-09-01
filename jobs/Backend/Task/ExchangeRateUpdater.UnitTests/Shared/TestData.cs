using ExchangeRateUpdater.Caching;
using ExchangeRateUpdater.Provider.Cnb.Dtos;
using ExchangeRateUpdater.Services.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace ExchangeRateUpdater.UnitTests.Shared
{
    public static class TestData
    {
        public static RatesStore CreateRatesStore()
        {
            return new RatesStore(
                new MemoryCache(new MemoryCacheOptions()),
                NullLogger<RatesStore>.Instance);
        }

        public static CnbResponse CreateRatesResponse(string date, IEnumerable<CnbRate> rates)
        {
            var list = rates.Select(r => new CnbRate
            {
                ValidFor = date,
                CurrencyCode = r.CurrencyCode,
                Amount = r.Amount,
                Rate = r.Rate
            }).ToList();

            return new CnbResponse { Rates = list };
        }

        public static CnbRate CreateRate(string code, int amount, decimal rate, string validFor)
        {
            return new CnbRate
            {
                CurrencyCode = code,
                Amount = amount,
                Rate = rate,
                ValidFor = validFor
            };
        }

        public static ExchangeRateProvider CreateProviderReturning(CnbResponse response)
        {
            var store = Substitute.For<IRatesStore>();
            store.Get().Returns(response);
            return new ExchangeRateProvider(store);
        }
    }
}
