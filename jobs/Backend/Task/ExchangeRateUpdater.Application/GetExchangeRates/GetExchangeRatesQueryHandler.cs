using ExchangeRateUpdater.Application.Common.Extensions;
using ExchangeRateUpdater.Domain;
using MediatR;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, GetExchangeRatesResponse>
{
    private readonly ICzechNationalBankExchangeRateClient _exchangeRateClient;
    private readonly ICzechNationalBankExchangeRateClientResponseConverter _clientResponseConverter;

    public GetExchangeRatesQueryHandler(ICzechNationalBankExchangeRateClient exchangeRateClient, ICzechNationalBankExchangeRateClientResponseConverter clientResponseConverter)
    {
        _exchangeRateClient = exchangeRateClient;
        _clientResponseConverter = clientResponseConverter;
    }

    public async Task<GetExchangeRatesResponse> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        if (request.CurrencyCodes.IsNullOrEmpty()) return new();
        Validate(request);

        var dateToRequest = GetDate(request);
        var clientResponse = await _exchangeRateClient.GetAsync(dateToRequest);
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
}