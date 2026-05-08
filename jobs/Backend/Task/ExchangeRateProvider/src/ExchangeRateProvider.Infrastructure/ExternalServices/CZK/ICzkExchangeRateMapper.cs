using ExchangeRateProvider.Domain.ValueObjects;

namespace ExchangeRateProvider.Infrastructure.ExternalServices.CZK;

public interface ICzkExchangeRateMapper
{
    IList<ExchangeRate> MapToExchangeRates(CzkExchangeRateResponse? response, Currency baseCurrency);
}
