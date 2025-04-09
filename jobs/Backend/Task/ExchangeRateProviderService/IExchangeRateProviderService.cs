using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;

namespace ExchangeRateProviderService;

public interface IExchangeRateProviderService
{
    Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(IEnumerable<CurrencyDto> currencies);
}
