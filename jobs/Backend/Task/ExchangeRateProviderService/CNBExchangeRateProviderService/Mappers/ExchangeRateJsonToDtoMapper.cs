using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Constants;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProviderService.CNBExchangeRateProviderService.Mappers;

internal interface IExchangeRateJsonToDtoMapper
{
    public IEnumerable<ExchangeRateDto> ExchangeRateDtoMapper(string exchangeRateJson);
}

internal class ExchangeRateJsonToDtoMapper : IExchangeRateJsonToDtoMapper
{
    private class ExchangeRateItem
    {
        public DateTime ValidFor { get; set; }

        public int Order { get; set; }

        public string? Country { get; set; }

        public string? Currency { get; set; }

        public int Amount { get; set; }

        public string? CurrencyCode { get; set; }

        public decimal Rate { get; set; }
    }

    private class ExchangeRateResponse
    {
        public IList<ExchangeRateItem> Rates { get; set; } = new List<ExchangeRateItem>();
    }

    private ILogger<ExchangeRateJsonToDtoMapper> _logger;

    public ExchangeRateJsonToDtoMapper(ILogger<ExchangeRateJsonToDtoMapper> logger)
    {
        _logger = logger;
    }

    public IEnumerable<ExchangeRateDto> ExchangeRateDtoMapper(string exchangeRateJson)
    {
        try
        {
            var deserializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var exchangeRateResponse = JsonSerializer
                .Deserialize<ExchangeRateResponse>(exchangeRateJson, deserializeOptions);

            if (exchangeRateResponse?.Rates == null)
            {
                _logger.LogError("_Deserialized exchange rates was null");
                return Enumerable.Empty<ExchangeRateDto>();
            }

            var exchangeRates = new List<ExchangeRateDto>();

            foreach(var exchangeRateItem in exchangeRateResponse.Rates)
            { 
                exchangeRates.Add(ExchangeRateResponseToExchangeRateDto(exchangeRateItem));
            }

            return exchangeRates;
        } catch (Exception ex)
        {
            _logger.LogError($"_Exchange rates json deserialization failed with exception: {ex.Message}");
            return Enumerable.Empty<ExchangeRateDto>();
        }
    }

    private static ExchangeRateDto ExchangeRateResponseToExchangeRateDto(ExchangeRateItem rateResponse)
    {
        return new ExchangeRateDto
        {
            BaseCurrency = Defaults.CURRENCY.BaseCurrency,
            TargetCurrency = new CurrencyDto { Code = rateResponse.CurrencyCode ?? string.Empty },
            Date = rateResponse.ValidFor,
            Rate = rateResponse.Rate,
        };
    }
}
