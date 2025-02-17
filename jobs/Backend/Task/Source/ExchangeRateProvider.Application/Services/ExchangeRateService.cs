namespace ExchangeRateProvider.Application.Services;

using Interfaces;
using Domain.DTOs;
using Domain.Entities;

public class ExchangeRateService(IExchangeRateProvider exchangeRateProvider) : IExchangeRateService
{
    public const string LocalCurrencyCode = "CZK";

    private readonly IExchangeRateProvider _exchangeRateProvider =
        exchangeRateProvider ?? throw new ArgumentNullException(nameof(exchangeRateProvider));

    public async Task<ExchangeRateResult> GetExchangeRatesAsync(IEnumerable<string> requestedCurrencies)
    {
        ArgumentNullException.ThrowIfNull(requestedCurrencies);

        var requestedCurrenciesWithoutLocal = requestedCurrencies.Where(c => !c.Equals(LocalCurrencyCode));

        var allRates = await _exchangeRateProvider.GetExchangeRatesAsync();

        var availableCurrencies = allRates.ToHashSet();
        var validRates = availableCurrencies
            .Where(currency => requestedCurrenciesWithoutLocal.Contains(currency.SourceCurrency.Code))
            .Select(r =>
                new ExchangeRate(new Currency(r.SourceCurrency.Code), new Currency(r.TargetCurrency.Code), r.Value))
            .ToList();
        var invalidCurrencies = requestedCurrenciesWithoutLocal.Where(reqCurrency =>
            !availableCurrencies.Any(currency => currency.SourceCurrency.Code.Equals(reqCurrency)));

        return new ExchangeRateResult(validRates, invalidCurrencies);
    }
}
