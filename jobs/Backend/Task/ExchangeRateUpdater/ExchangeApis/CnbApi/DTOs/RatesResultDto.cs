using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.ExchangeApis.CnbApi.DTOs;

public record RatesResultDto
{
    public List<RateDto> Rates { get; init; }

    public IEnumerable<ExchangeRate> ToExchangeRates(string targetCurrency = "CZK")
    {
        return Rates.Select(r => r.ToExchangeRate(targetCurrency));
    }
}

public record RateDto
{
    public int Amount { get; init; }
    public string Country { get; init; }
    public string Currency { get; init; }
    public string CurrencyCode { get; init; }
    public int Order { get; init; }
    public decimal Rate { get; init; }
    public string ValidFor { get; init; }

    public ExchangeRate ToExchangeRate(string targetCurrencyCode = "CZK")
    {
        return new ExchangeRate(CurrencyCode, targetCurrencyCode, Rate / Amount);
    }
}
