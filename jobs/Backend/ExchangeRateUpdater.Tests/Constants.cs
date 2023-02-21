using ExchangeRateUpdater.Data;

namespace ExchangeRateUpdater.Tests;

public static class Constants
{
    public static readonly decimal EURCZK_VALUE = 23.6M;
    public static readonly decimal USDEUR_VALUE = 0.94M;
    public static readonly decimal EURJPY_VALUE = 143.26M;
    public static readonly Currency EUR = new Currency("EUR");
    public static readonly Currency CZK = new Currency("CZK");
    public static readonly Currency JPY = new Currency("JPY");
    public static readonly Currency USD = new Currency("USD");
    public static readonly ExchangeRate EURCZK = new ExchangeRate(EUR, CZK, EURCZK_VALUE);
    public static readonly ExchangeRate USDEUR = new ExchangeRate(USD, EUR, USDEUR_VALUE);
    public static readonly ExchangeRate EURJPY = new ExchangeRate(EUR, JPY, EURJPY_VALUE);

}
