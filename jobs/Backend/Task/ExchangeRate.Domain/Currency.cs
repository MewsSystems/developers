using ExchangeRate.Models.Utils;

namespace ExchangeRate.Models;

public class Currency
{
    #region Constructors

    public Currency(string code)
    {
        CurrencyUtils.Validate(code);

        Code = code;
    }

    #endregion

    #region Properties

    /// <summary>
    ///     Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; }

    #endregion

    public override string ToString()
    {
        return Code;
    }
}