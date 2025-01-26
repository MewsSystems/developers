using Application.Abstractions;
using Domain.Models;

namespace Application.UseCases.ExchangeRates;

public sealed record GetExchangeRateRequest()
{
    public GetExchangeRateQuery GetQuery(string language, string sourceCurrency, string targetCurrency)
        => new GetExchangeRateQuery(language.ToUpper(), sourceCurrency.ToUpper(), targetCurrency.ToUpper());
}

public sealed record GetExchangeRateQuery(
    string Language,
    string SourceCurrency, 
    string TargetCurrency
    ) : IQuery<GetExchangeRateResponse> { }

public sealed record GetExchangeRateResponse(ExchangeRate ExchangeRate);
