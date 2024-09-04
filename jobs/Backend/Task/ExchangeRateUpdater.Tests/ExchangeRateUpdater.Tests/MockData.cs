namespace ExchangeRateUpdater.Tests;

public static class MockData
{
    public static class Currencies
    {
        public static readonly HashSet<Currency> EmptyCurrencies = [];
        public static readonly HashSet<Currency> DefaultCurrencies = [
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        ];
        public static readonly HashSet<Currency> NonExistingCurrency = [
            new Currency("NONEXISTING")
        ];
    }

    public static class ResponseBodies
    {
        public static readonly string Empty = File.ReadAllText("ResponseBodies/empty.json");
        public static readonly string Default = File.ReadAllText("ResponseBodies/default.json");
    }
}