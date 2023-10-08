using System.Collections.Generic;

namespace ExchangeRateUpdater.Application
{
    public record CnbExchangeRateResponse(IEnumerable<CnbExchangeRate> Rates);
    public record CnbExchangeRate(long Amount, string CurrencyCode, decimal Rate);
}
