using ExchangeRateUpdater.Common.Models;

namespace ExchangeRateUpdater.Services;

public static class Utils
{
    public static List<Currency> SourceCurrencies =>
    [
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
}

