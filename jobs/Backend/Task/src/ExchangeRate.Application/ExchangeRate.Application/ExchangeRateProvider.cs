using ExchangeRate.Application.Services;
using ExchangeRate.Domain;

namespace ExchangeRate.Application;

public interface IExchangeRateProvider
{
    Task<Domain.ExchangeRate?> GetExchangeRate(Currency requestedCurrency);
    Task<IEnumerable<Domain.ExchangeRate>> GetExchangeRates(IEnumerable<Currency> requestedCurrencies);
}

public class ExchangeRateProvider(IExchangeRatesService exchangeRatesService) : IExchangeRateProvider
{
    private readonly IExchangeRatesService _exchangeRatesService = exchangeRatesService;
    
    public async Task<Domain.ExchangeRate?> GetExchangeRate(Currency requestedCurrency)
    {
        var exchangeRates = await _exchangeRatesService.GetCurrentExchangeRates();
        var requestedExchangeRate = exchangeRates.FirstOrDefault(rate => rate.SourceCurrency == requestedCurrency); //Note: here is an opportunity to improve efficiency by using a dictionary for faster lookups

        return requestedExchangeRate;
    }
    
    public async Task<IEnumerable<Domain.ExchangeRate>> GetExchangeRates(IEnumerable<Currency> requestedCurrencies)
    {
        var exchangeRates = await _exchangeRatesService.GetCurrentExchangeRates();
        var requestedExchangeRate = exchangeRates.Where(rate => requestedCurrencies.Any(requestedCurrency => requestedCurrency.Code == rate.SourceCurrency.Code));

        return requestedExchangeRate;
    }
}