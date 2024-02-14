namespace ExchangeRatesService.Models;

public static class CurrencyIterator
{
    public static IEnumerable<Currency> Currencies { get; } = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };
}