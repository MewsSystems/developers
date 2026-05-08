using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Domain.ValueObjects;

namespace ExchangeRateProvider.Infrastructure.ExternalServices.CZK;

public class CzkExchangeRateService(ICzkApiClient apiClient, ICzkExchangeRateMapper mapper) : IExchangeRateService
{
    public async Task<IList<ExchangeRate>> GetExchangeRatesAsync(Currency baseCurrency, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetExchangeRatesAsync(cancellationToken);
        var exchangeRates = mapper.MapToExchangeRates(response, baseCurrency);

        return exchangeRates;
    }
}
