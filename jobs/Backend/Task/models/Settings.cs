using System.Collections.Generic;

public sealed class Settings
{
    public string BaseCurrency { get; set; }
    public List<string> SupportedCurrencies { get; set; }

    public string BankRatesUrl {get; set; }

    public int ExchangePrecision {get; set;} = 3;
}