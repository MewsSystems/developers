namespace Adapter.ExchangeRateProvider.CzechNationalBank.Dtos;

internal class ExchangeDataRawDto
{
    internal int Amount { get; set; }
    internal string? CurrencyCode { get; set; }
    internal decimal Rate { get; set; }
}
