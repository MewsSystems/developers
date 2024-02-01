namespace ExchangeRateUpdaterApi.Dtos.Request;

/// <summary>
/// Dto containing information on the pair SourceCurrency/TargetCurrency to request
/// </summary>
public class ExchangeRateDetailsDto
{
    /// <summary>
    /// Information on SourceCurrency
    /// </summary>
    public CurrencyDto SourceCurrency { get; set; }
    /// <summary>
    /// Information on TargetCurrency
    /// </summary>
    public CurrencyDto TargetCurrency { get; set; }
}