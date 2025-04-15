namespace ExchangeRateUpdater.Domain.Options;

public class CurrencyOptions
{
    public string BaseCurrency { get; set; }
    public string[] Currencies { get; set; }
}