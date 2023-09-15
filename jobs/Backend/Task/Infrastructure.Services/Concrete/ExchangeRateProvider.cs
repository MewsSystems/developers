using Infrastructure.Models.Constants;
using Infrastructure.Models.Responses;
using Infrastructure.Services.Abstract;

namespace Infrastructure.Services.Concrete;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IBankDataService _czechNationalBankDataService;

    public ExchangeRateProvider(IBankDataService czechNationalBankDataService)
    {
        _czechNationalBankDataService = czechNationalBankDataService;
    }

    public async Task<IEnumerable<ExchangeRate>?> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var currencyRates = await _czechNationalBankDataService.GetExchangeRates();

        if(currencyRates == null)
        {
            return null;
        }

        var defaultCurrency = _czechNationalBankDataService.GetDefaultCurrency();

        var currencyCodesSet = currencies.Select(x => x.Code.ToUpper()).ToHashSet();
        var rates = new List<ExchangeRate>();
        foreach (var currencyRate in currencyRates.Where(x => currencyCodesSet.Contains(x.CurrencyCode.ToUpper())))
        {
            rates.Add(new ExchangeRate(new Currency(currencyRate.CurrencyCode), defaultCurrency, Math.Round(currencyRate.Amount / currencyRate.Rate, RoundingConstants.NumberOfDecimalPlaces)));
        }
        return rates;
    }
}
