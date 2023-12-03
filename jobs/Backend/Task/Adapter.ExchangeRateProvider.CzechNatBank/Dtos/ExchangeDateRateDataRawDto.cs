namespace Adapter.ExchangeRateProvider.CzechNatBank.Dtos;

internal class ExchangeDateRateDataRawDto
{
    internal int Amount { get; set; }
    internal string? CurrencyCode { get; set; }
    internal decimal Rate { get; set; }
    internal DateTime? DateTime { get; set; }
}
