namespace ExchangeRateUpdater.Domain.Models;

public enum CnbLanguage
{
    EN,
    CZ
}

public class CzechNationalBankOptions
{
    public const string SectionName = "CzechNationalBank";
    public string BaseUrl { get; set; } = "https://api.cnb.cz/cnbapi/exrates/daily";
    public string DateFormat { get; set; } = "yyyy-MM-dd";
    public CnbLanguage Language { get; set; } = CnbLanguage.EN;
}
