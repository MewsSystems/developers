namespace ExchangeRateProvider.BankApiClients.Cnb.Models;

public class CnbBankCurrencyRate
{
    public string? ValidFor { get; set; }
    public int? Order { get; set; }
    public string? Country { get; set; }
    public string? Currency { get; set; }
    public long? Amount { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal? Rate { get; set; }
}