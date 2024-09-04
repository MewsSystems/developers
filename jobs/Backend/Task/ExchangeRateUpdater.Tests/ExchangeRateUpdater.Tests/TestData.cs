namespace ExchangeRateUpdater.Tests;

public static class TestData
{
    public static HashSet<Currency> EmptyCurrencies = [];
    public static HashSet<Currency> Currencies = [
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
    public static HashSet<Currency> NonExistingCurrency = [
        new Currency("NONEXISTING")
    ];
}