using Application.Abstractions;
using Domain.Abstractions;
using Domain.Abstractions.Data;
using Domain.Errors;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ExchangeRates;

public sealed class GetDailyExchangeRateQueryHandler : IQueryHandler<GetDailyExchangeRateQuery, GetDailyExchangeRateResponse>
{
    private readonly ILogger<GetDailyExchangeRateQueryHandler> _logger;
    private readonly IAvailableCurrencies _currencyData;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetDailyExchangeRateQueryHandler(
        ILogger<GetDailyExchangeRateQueryHandler> logger,
        IAvailableCurrencies currencyData,
        IExchangeRateProvider exchangeRateProvider)
    {
        _logger = logger;
        _currencyData = currencyData;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<Result<GetDailyExchangeRateResponse>> Handle(GetDailyExchangeRateQuery request, CancellationToken cancellationToken)
    {
        var sourceCurrency = _currencyData.GetCurrencyWithCode(request.CurrencyCode);

        if (sourceCurrency is null)
        {
            return Result.Fail(new CurrencyNotFound(_logger, request.CurrencyCode)); 
        }

        var data = await _exchangeRateProvider.GetDailyExchangeRates(sourceCurrency, request.Language);

        return Result.Ok(new GetDailyExchangeRateResponse(data));
    }
}
