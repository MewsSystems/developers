namespace ExchangeRate.Provider.Cnb.Models;

public class CnbExchangeRate
{
    #region Constructors

    public CnbExchangeRate(string? country, string? currency, int? amount, string? code, decimal? rate)
    {
        Country = country;
        Currency = currency;
        Amount = amount;
        Code = code;
        Rate = rate;
    }

    #endregion

    #region Properties

    public int? Amount { get; }
    public string? Code { get; }
    public string? Country { get; }
    public string? Currency { get; }
    public decimal? Rate { get; }

    #endregion
}