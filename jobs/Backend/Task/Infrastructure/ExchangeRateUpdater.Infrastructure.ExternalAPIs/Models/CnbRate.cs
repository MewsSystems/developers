using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Models;

internal record CnbRate(
    int Amount,
    string Country,
    string Currency,
    string CurrencyCode,
    int Order,
    decimal Rate,
    DateTime ValidFor)
{
    public ExchangeRate ToExchangeRate(string targetCurrency)
    {
        var ratePerOne = Rate / Amount;
        return new ExchangeRate(new Currency(CurrencyCode), new Currency(targetCurrency), ratePerOne);
    }
} 