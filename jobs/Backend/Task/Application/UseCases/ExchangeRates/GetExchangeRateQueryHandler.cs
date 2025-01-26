using Application.Abstractions;
using Domain.Abstractions;
using Domain.Abstractions.Data;
using Domain.Errors;
using FluentResults;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ExchangeRates;

public sealed class GetExchangeRateQueryHandler : IQueryHandler<GetExchangeRateQuery, GetExchangeRateResponse>
{
    private readonly ILogger<GetExchangeRateQueryHandler> _logger;
    private readonly IAvailableCurrencies _currencyData;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetExchangeRateQueryHandler(
        ILogger<GetExchangeRateQueryHandler> logger, 
        IAvailableCurrencies currencyData, 
        IExchangeRateProvider exchangeRateProvider)
    {
        _logger = logger;
        _currencyData = currencyData;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<Result<GetExchangeRateResponse>> Handle(GetExchangeRateQuery request, CancellationToken cancellationToken)
    {
        var sourceCurrency = _currencyData.GetCurrencyWithCode(request.SourceCurrency);

        if (sourceCurrency is null)
        {
            return Result.Fail(new CurrencyNotFound(_logger, request.SourceCurrency));
        }

        var targetCurrency = _currencyData.GetCurrencyWithCode(request.TargetCurrency);

        if (sourceCurrency is null)
        {
            return Result.Fail(new CurrencyNotFound(_logger, request.TargetCurrency));
        }

        var exchangeRate = await _exchangeRateProvider.GetExchangeRate(sourceCurrency, targetCurrency, request.Language);

        if (exchangeRate is null)
        {
            return Result.Fail(new ExchangeRateNotFound(_logger, request.SourceCurrency, request.TargetCurrency));
        }

        return Result.Ok(new GetExchangeRateResponse(exchangeRate));
    }
}
