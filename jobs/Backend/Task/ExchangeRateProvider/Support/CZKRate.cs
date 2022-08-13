namespace ExchangeRateUpdater.Support;

public record CZKRate(string currency, decimal rate)
{
  public static implicit operator Currency(CZKRate rate)
  {
    return new Currency(rate.currency);
  }   
};