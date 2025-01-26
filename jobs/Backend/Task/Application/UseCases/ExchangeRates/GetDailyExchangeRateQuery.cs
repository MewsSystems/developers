using Application.Abstractions;
using Domain.Models;

namespace Application.UseCases.ExchangeRates;

public sealed record GetDailyExchangeRateRequest()
{
    public GetDailyExchangeRateQuery GetQuery(string currencyCode, string language)
        => new GetDailyExchangeRateQuery(currencyCode.ToUpper(), language.ToUpper());
}

public sealed record GetDailyExchangeRateQuery(string CurrencyCode, string Language) : IQuery<GetDailyExchangeRateResponse> { }

public sealed record GetDailyExchangeRateResponse(List<ExchangeRate> ExchangeRates);
