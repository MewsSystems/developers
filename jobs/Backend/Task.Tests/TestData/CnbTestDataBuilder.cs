using ExchangeRateUpdater.Parsers.Models;

namespace ExchangeRateUpdater.Tests.TestData
{
    // Provides reusable test data for CnbExchangeRateData mocking.
    public static class CnbTestDataBuilder
    {
        public static CnbExchangeRateData USD() => new("USA", "dollar", 1, "USD", 20.774m);

        public static CnbExchangeRateData EUR() => new("EMU", "euro", 1, "EUR", 24.280m);

        public static CnbExchangeRateData JPY() => new("Japan", "yen", 100, "JPY", 13.278m);
        
        public static CnbExchangeRateData GBP() => new("United Kingdom", "pound", 1, "GBP", 28.022m);

        public static CnbExchangeRateData IDR() => new("Indonesia", "rupiah", 1000, "IDR", 1.238m);
    }
}
