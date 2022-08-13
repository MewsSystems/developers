namespace ExchangeRateUpdater.Support;

/// <summary>
/// Support type to store CZK exchange rates  
/// </summary>
/// <param name="Currency">Currency code</param>
/// <param name="Rate">Exchange rate (as "XYZ/CZK")</param>
public record CZKRate(string Currency, decimal Rate)
{
  public static implicit operator Currency(CZKRate rate)
  {
    return new Currency(rate.Currency);
  }   
};