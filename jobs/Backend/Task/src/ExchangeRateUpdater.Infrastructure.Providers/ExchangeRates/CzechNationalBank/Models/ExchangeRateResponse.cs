using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Infrastructure.Providers.ExchangeRates.CzechNationalBank.Models;

public class CnbExchangeRateResponse
{
    public CnbExchangeRateModel[] Rates { get; set; }
}

public class CnbExchangeRateModel
{
    public int Amount { get; set; }
    public string Country { get; set; }
    public string Currency { get; set; }
    public string CurrencyCode { get; set; }
    public int Order { get; set; }
    public decimal Rate { get; set; }
    public string ValidFor { get; set; }
    
    public ExchangeRate ToExchangeRate()
    {
        // Todo Andrei: Alamin panao implement yung valid for
        return new ExchangeRate(new Currency(CurrencyCode), new Currency("CZK"), Rate, "CNB", DateTime.Parse(ValidFor));
    }
}