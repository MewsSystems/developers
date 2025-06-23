using System;

namespace ExchangeRateUpdater.ExchangeRateProviders.CzechNationalBank;

public class CzechNationalBankExchangeRate
{
    public decimal Amount { get; set; }
    public string Country { get; set; }
    public string Currency { get; set; }
    public string CurrencyCode { get; set; }
    public decimal Rate { get; set; }
    public DateTime ValidFor { get; set; }
}