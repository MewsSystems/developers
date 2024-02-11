namespace ExchangeRate.Infrastructure.Cnb.Mappers;

public static class ExchangeRatesMapper
{
    public static IEnumerable<Domain.ExchangeRate> ToDomain(this IEnumerable<Models.ExchangeRate> exchangeRates)
    {
        return exchangeRates.Select(x => new Domain.ExchangeRate(
            sourceCurrency: new Domain.Currency(x.CurrencyCode),
            targetCurrency: new Domain.Currency("CZK"),
            value: (decimal)x.Rate
        ));
    }
}