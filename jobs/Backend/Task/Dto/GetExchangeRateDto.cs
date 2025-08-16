using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public record GetExchangeRatesDto(IEnumerable<ExchangeRateDto> Rates);
    public record ExchangeRateDto(string Currency, string CurrencyCode, int Amount, decimal Rate);
}