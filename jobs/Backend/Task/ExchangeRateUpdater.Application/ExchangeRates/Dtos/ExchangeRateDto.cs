namespace ExchangeRateUpdater.Application.ExchangeRates.Dtos;

using Common.Mappings;
using Domain.Entities;

public record ExchangeRateDto
{
    /// <summary>
    /// Source currency of the exchange rate.
    /// </summary>
    public string SourceCurrencyCode { get; set; }

    /// <summary>
    /// Target currency of the exchange rate.
    /// </summary>
    public string TargetCurrencyCode { get; set; }

    /// <summary>
    /// Value of the exchange rate from 1 unit of the source currency to the target currency.
    /// </summary>
    public decimal Value { get; set; }

    public sealed override string ToString() => $"{SourceCurrencyCode}/{TargetCurrencyCode}={Value}";
}