namespace ExchangeRateService.Client.Model.CNB;

public class ExchangeRateBody
{

    public string ValidFor { get; set; } = String.Empty;
    
    public int Order { get; set; }

    public string Country { get; set; } = String.Empty;

    public string Currency { get; set; } = String.Empty;
    
    public int Amount { get; set; }
    
    public string CurrencyCode { get; set; } = String.Empty;
    
    public decimal Rate { get; set; }
}