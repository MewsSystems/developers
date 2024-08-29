using ExchangeRateUpdater.Application.Cache;
using ExchangeRateUpdater.Application.Common.Extensions;
using ExchangeRateUpdater.Domain;
using MediatR;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, GetExchangeRatesResponse>
{
    private readonly ICzechNationalBankExchangeRateClient _exchangeRateClient;
    private readonly ICzechNationalBankExchangeRateClientResponseConverter _clientResponseConverter;
    private readonly IRedisClient _redisClient;

    public GetExchangeRatesQueryHandler(
        ICzechNationalBankExchangeRateClient exchangeRateClient,
        ICzechNationalBankExchangeRateClientResponseConverter clientResponseConverter,
        IRedisClient redisClient
    )
    {
        _exchangeRateClient = exchangeRateClient;
        _clientResponseConverter = clientResponseConverter;
        _redisClient = redisClient;
    }

    public async Task<GetExchangeRatesResponse> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        if (request.CurrencyCodes.IsNullOrEmpty()) return new();
        Validate(request);

        var clientResponse = await GetClientResponseAsync(request);
        var ratesFromResponse = _clientResponseConverter.Convert(clientResponse);
        var ratesForRequestedCurrencies = ratesFromResponse.Where(c => request.CurrencyCodes!.Contains(c.SourceCurrency.Code));
        return new()
        {
            ExchangeRates = ratesForRequestedCurrencies.Select(rate => new ExchangeRateDto
            {
                SourceCurrency = rate.SourceCurrency.Code,
                TargetCurrency = rate.TargetCurrency.Code,
                Rate = rate.Value
            }).ToList()
        };
    }

    private static void Validate(GetExchangeRatesQuery request)
    {
        foreach (var currencyCode in request.CurrencyCodes!)
        {
            try
            {
                _ = new Currency(currencyCode);
            }
            catch (ArgumentException argumentException) when (argumentException.ParamName == nameof(Currency.Code))
            {
                throw new InvalidCurrencyCodeException(currencyCode, argumentException);
            }
        }
    }

    private static DateOnly GetDate(GetExchangeRatesQuery request)
    {
        if (request.Date.HasValue)
        {
            return request.Date.Value;
        }

        return DateOnly.FromDateTime(DateTime.UtcNow);
    }

    private async Task<string?> GetClientResponseAsync(GetExchangeRatesQuery request)
    {
        var dateToRequest = GetDate(request);
        var cacheKey = dateToRequest.ToString();
        var callback = () => _exchangeRateClient.GetAsync(dateToRequest);
        var expiration = TimeSpan.FromHours(3);
        
        var response = await _redisClient.GetAsync(cacheKey, callback, expiration);
        return response;
    }
}