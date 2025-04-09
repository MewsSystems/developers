namespace ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;

public struct ExchangeRateDto
{
    public CurrencyDto BaseCurrency;

    public CurrencyDto TargetCurrency;
    
    public decimal Rate;
    
    public DateTime Date;
}
