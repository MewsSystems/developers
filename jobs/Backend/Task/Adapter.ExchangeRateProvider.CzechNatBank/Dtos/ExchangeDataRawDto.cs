namespace Adapter.ExchangeRateProvider.CzechNatBank.Dtos;

internal class ExchangeRateDataRawDto
{
    internal int Amount { get; set; }
    internal string? CurrencyCode { get; set; }
    internal decimal Rate { get; set; }
}
