namespace ExchangeRateUpdater.Domain.ValueObjects;

/// <summary>
/// Currency class that validates and stores currency code.
/// </summary>
public class Currency
{
    public string CurrencyCode { get; }

    /// <summary>
    /// Main constructor for Currency that assigns a currency code to a currency. 
    /// Note: Currencies with the same codes are considered the same currency.
    /// </summary>
    /// <param name="currencyCode">Currency Code according to ISO 4217</param>
    /// <exception cref="ArgumentNullException">In case currencyCode is empty, whitespace, or null will throw ArgumentNullException</exception>
    /// <exception cref="FormatException">In case currencyCode is not ISO 4217 will throw FormatException</exception>
    public Currency(string? currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            throw new ArgumentNullException("currencyCode cannot be empty, null or whitespaces");
        }

        if (currencyCode.Length != 3 || IsCurrencyCodeInactive(currencyCode)) throw new FormatException("currencyCode has to be of ISO 4217 format and considered active according to https://en.wikipedia.org/wiki/ISO_4217");


        CurrencyCode = currencyCode;
    }

    public static implicit operator string(Currency currency) => currency.CurrencyCode;

    private static bool IsCurrencyCodeActive(string currencyCode) => AcceptedCurrencyCodes.Contains(currencyCode);

    private static bool IsCurrencyCodeInactive(string currencyCode) => IsCurrencyCodeActive(currencyCode) == false;

    public static bool operator ==(Currency left, Currency right) => left.Equals(right);
    public static bool operator !=(Currency left, Currency right) => !(left == right);

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Currency currency && currency.CurrencyCode.Equals(this.CurrencyCode);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return CurrencyCode.GetHashCode();
    }

    /// <summary>
    /// I added this private variable to the end of the class because it is not that important to pay attention to.
    /// It will contain the active codes of ISO 4217 according to https://en.wikipedia.org/wiki/ISO_4217
    /// </summary>
    private static HashSet<string> AcceptedCurrencyCodes = new HashSet<string>
    {
        "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZM", "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOV",
        "BRL", "BSD", "BTN", "BWP", "BYR", "BZD", "CAD", "CDF", "CHF", "CLF", "CLP", "CNY", "COP", "COU", "CRC", "CSD", "CUP", "CVE", "CYP", "CZK",
        "DJF", "DKK", "DOP", "DZD", "EEK", "EGP", "ERN", "ETB", "EUR", "FJD", "FKP", "GBP", "GEL", "GHC", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD",
        "HNL", "HRK", "HTG", "HUF", "IDR", "ILS", "INR", "IQD", "IRR", "ISK", "JMD", "JOD", "JPY", "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD",
        "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MTL", "MUR",
        "MVR", "MWK", "MXN", "MXV", "MYR", "MZM", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG",
        "QAR", "RON", "RUB", "RWF", "SAR", "SBD", "SCR", "SDD", "SEK", "SGD", "SHP", "SKK", "SLL", "SOS", "SRD", "STD", "SVC", "SYP", "SZL", "THB",
        "TJS", "TMM", "TND", "TOP", "TPE", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "USD", "USN", "USS", "UYU", "UZS", "VEB", "VND", "VUV", "WST",
        "XAF", "XAG", "XAU", "XBA", "XBB", "XBC", "XBD", "XCD", "XDR", "XFO", "XFU", "XOF", "XPD", "XPF", "XPT", "XXX", "YER", "ZAR", "ZMK", "ZWD"
    };
}

