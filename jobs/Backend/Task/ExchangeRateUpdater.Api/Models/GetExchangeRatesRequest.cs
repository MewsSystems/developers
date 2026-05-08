using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Api.Models;

public record GetExchangeRatesRequest
{
    public IEnumerable<string> CurrencyCodes { get; init; } = Array.Empty<string>();
    public DateTime? Date { get; init; }

    public IEnumerable<Currency> ToCurrencies() => CurrencyCodes.Select(code => new Currency(code));
}