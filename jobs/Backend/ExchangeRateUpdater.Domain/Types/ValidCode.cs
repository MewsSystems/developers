using System.Text.RegularExpressions;

namespace ExchangeRateUpdater.Domain.Types;

public class ValidCode
{
    private static readonly Regex Iso4217RegEx = new(
        @"/^AED|AFN|ALL|AMD|ANG|AOA|ARS|AUD|AWG|AZN|BAM|BBD|BDT|BGN|BHD|BIF|BMD|BND|BOB|BRL|BSD|BTN|BWP|BYR|BZD|CAD|CDF|CHF|CLP|CNY|COP|CRC|CUC|CUP|CVE|CZK|DJF|DKK|DOP|DZD|EGP|ERN|ETB|EUR|FJD|FKP|GBP|GEL|GGP|GHS|GIP|GMD|GNF|GTQ|GYD|HKD|HNL|HRK|HTG|HUF|IDR|ILS|IMP|INR|IQD|IRR|ISK|JEP|JMD|JOD|JPY|KES|KGS|KHR|KMF|KPW|KRW|KWD|KYD|KZT|LAK|LBP|LKR|LRD|LSL|LYD|MAD|MDL|MGA|MKD|MMK|MNT|MOP|MRO|MUR|MVR|MWK|MXN|MYR|MZN|NAD|NGN|NIO|NOK|NPR|NZD|OMR|PAB|PEN|PGK|PHP|PKR|PLN|PYG|QAR|RON|RSD|RUB|RWF|SAR|SBD|SCR|SDG|SEK|SGD|SHP|SLL|SOS|SPL|SRD|STD|SVC|SYP|SZL|THB|TJS|TMT|TND|TOP|TRY|TTD|TVD|TWD|TZS|UAH|UGX|USD|UYU|UZS|VEF|VND|VUV|WST|XAF|XCD|XDR|XOF|XPF|YER|ZAR|ZMW|ZWD$/");

    public string? Value { get; }
    private ValidCode(string code)
    {
        if (Iso4217RegEx.IsMatch(code))
        {
            Value = code;
        }
    }
    public static ValidCode? Create(string code)
    {
        return !string.IsNullOrWhiteSpace(code) && Iso4217RegEx.IsMatch(code) ? new ValidCode(code) : null;
    }
    public static ValidCode CreateUnsafe(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("You cannot create a valid code from whitespace, empty string or null.");
        }
        if (!Iso4217RegEx.IsMatch(code))
        {
            throw new ArgumentException("You cannot create a valid code from a non ISO-4217 code.");
        }
        return new ValidCode(code);
    }

}