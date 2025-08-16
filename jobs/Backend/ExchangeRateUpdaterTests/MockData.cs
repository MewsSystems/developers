using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdaterTests
{
    public static class MockData
    {
        public const string AustralianCurrencyCode = "AUD";
        public const string BrasilianCurrencyCode = "BRL";
        public const string CzechCurrencyCode = "CZK";

        public static List<ExchangeRateRecord> ValidExchangeRateRecordList = new()
        {
            new ExchangeRateRecord()
            {
                ValidFor = "2023-08-25",
                Order = 164,
                Country = "Austrálie",
                Currency = "dolar",
                Amount = 1,
                CurrencyCode = AustralianCurrencyCode,
                Rate = 14.354M
            },
            new ExchangeRateRecord()
            {
                ValidFor = "2023-08-25",
                Order = 164,
                Country = "Brazílie",
                Currency = "real",
                Amount = 1,
                CurrencyCode = "BRL",
                Rate = 4.592M
            }
        };

        public static Currency AustralianCurrency = new(AustralianCurrencyCode);
        public static Currency BrasilianCurrency = new(BrasilianCurrencyCode);
        public static Currency CzechCurrency = new(CzechCurrencyCode);
    }
}
