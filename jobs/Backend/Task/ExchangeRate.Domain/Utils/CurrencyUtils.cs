using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ExchangeRate.Models.Utils;

public static class CurrencyUtils
{
    #region Properties

    /// <summary>
    ///     Regex string for ISO 4217
    /// </summary>
    public static string CurrencyRegexString => "^[a-zA-Z]{3}$";

    #endregion

    /// <summary>
    ///     Validates input string parameter if meets ISO 4217 requirements
    /// </summary>
    /// <param name="code">Currency code</param>
    public static void Validate(string code)
    {
        if (Regex.Match(code, CurrencyRegexString).Success == false)
            throw new ValidationException($"Input string {code} doesn't meets requirements for ISO 4217");
    }
}