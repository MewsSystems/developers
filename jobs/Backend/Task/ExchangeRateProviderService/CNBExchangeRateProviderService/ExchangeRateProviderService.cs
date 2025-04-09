using ExchangeRateProviderService.CNBExchangeRateProviderService.Client;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Mappers;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Validation;
using FluentValidation;

namespace ExchangeRateProviderService.CNBExchangeRateProviderService;

internal class ExchangeRateProviderService : IExchangeRateProviderService
{
    private readonly IApiClient _client;
    private readonly IExchangeRateJsonToDtoMapper _mapper;
    private readonly IValidator<ExchangeRateDto> _exchangeRateModelValidator;

    public ExchangeRateProviderService(
        IApiClient client, IExchangeRateJsonToDtoMapper mapper,
        IValidator<ExchangeRateDto> exchangeRateModelValidator)
    {
        _client = client;
        _mapper = mapper;
        _exchangeRateModelValidator = exchangeRateModelValidator;
    }

    public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(IEnumerable<CurrencyDto> currencies)
    {
        if (currencies == null
            || !currencies.Any())
        {
            return Enumerable.Empty<ExchangeRateDto>();
        }

        var exchangeRatesJson = await _client.GetDailyRatesJson();

        if (string.IsNullOrEmpty(exchangeRatesJson))
        {
            return Enumerable.Empty<ExchangeRateDto>();
        }

        var exchangeRates = _mapper.ExchangeRateDtoMapper(exchangeRatesJson);

        var validExchangeRates = new List<ExchangeRateDto>();

        foreach (var exchangeRate in exchangeRates)
        {
            if (_exchangeRateModelValidator.Validate(exchangeRate).IsValid
                && currencies.Any(currency => currency == exchangeRate.TargetCurrency))
            {
                validExchangeRates.Add(exchangeRate);
            }
        }

        return validExchangeRates;
    }
}
