using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Implementations.CzechNationalBank;

internal class ExchangeRateProviderCzechNationalBank : IExchangeRateProvider
{
    private readonly ICzechNationalBankApi _czechNationalBankApi;

    public ExchangeRateProviderCzechNationalBank(ICzechNationalBankApi czechNationalBankApi)
    {
        _czechNationalBankApi = czechNationalBankApi;
    }

    public async Task<ICollection<ExchangeRate>> GetExchangeRates(ICollection<Currency> currencies, DateTimeOffset date)
    {
        var response = await _czechNationalBankApi.GetExratesDaily(date);
        return response
            .Rates
            .Where(res => currencies.Any(curr => curr.Code.Equals(res.CurrencyCode, StringComparison.OrdinalIgnoreCase)))
            .Select(res =>
                new ExchangeRate(
                    new Currency("CZK"),
                    new Currency(res.CurrencyCode.ToUpperInvariant()),
                    res.Rate
                )
            ).ToArray();
    }
}