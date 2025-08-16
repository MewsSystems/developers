using Mews.ERP.AppService.Data.Models;

namespace Mews.ERP.AppService.Features.Fetch.Models;

public class ExchangeRate
{
    public Currency SourceCurrency { get; }

    public Currency TargetCurrency { get; }

    public int Amount { get; set; }

    public decimal Value { get; }
    
    public string ValidFor { get; set; }
    
    public string Country { get; set; }
    
    public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, int amount, decimal value, string validFor, string country)
    {
        SourceCurrency = sourceCurrency;
        TargetCurrency = targetCurrency;
        Amount = amount;
        Value = value;
        ValidFor = validFor;
        Country = country;
    }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value}";
    }
}