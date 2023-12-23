namespace ExchangeRateUpdater.Model.ExchangeRates;

/// <param name="IsoCode3">Three-letter ISO 4217 code of the currency.</param>
public record Currency(string IsoCode3)
{
    public static Currency Czk => new("CZK");
    
    public override string ToString()
    {
        return IsoCode3;
    }
}