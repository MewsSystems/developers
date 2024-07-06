using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Application.Services.Models;
using FluentValidation;
using MediatR;

namespace ExchangeRateUpdater.Application.Features.ExchangeRates.GetByCurrency;

internal class GetExchangeRatesByCurrencyQueryHandler : IRequestHandler<GetExchangeRatesByCurrencyQuery, GetExchangeRatesByCurrencyQueryResponse>
{
    private readonly IExternalExchangeRateProvider _exchangeRateProvider;
    private readonly IValidator<GetExchangeRatesByCurrencyQuery> _validator;

    public GetExchangeRatesByCurrencyQueryHandler(
        IExternalExchangeRateProvider exchangeRateService,
        IValidator<GetExchangeRatesByCurrencyQuery> validator)
    {
        _exchangeRateProvider = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<GetExchangeRatesByCurrencyQueryResponse> Handle(GetExchangeRatesByCurrencyQuery query, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(query);

        var getDailyExchangeRatesResponse = await _exchangeRateProvider.GetDailyExchangeRates(query.ForDate);

        var exchangeRates = GetApplicableExchangeRates(query.CurrencyCodes, getDailyExchangeRatesResponse);

        return new GetExchangeRatesByCurrencyQueryResponse(ExchangeRates: exchangeRates);
    }

    public IEnumerable<ExchangeRate> GetApplicableExchangeRates(
        IEnumerable<string> currencies,
        ExchangeRateProviderResponse dailyExchangeRates)
    {
        var applicableExchangeRates = dailyExchangeRates.Rates
            .Where(rate => currencies.Select(c => c.ToUpper()).Contains(rate.CurrencyCode.ToUpper()))
            .ToList();

        var exchangeRates = new List<ExchangeRate>();

        foreach (var exRate in applicableExchangeRates)
        {
            var exchangeRate = new ExchangeRate(
                sourceCurrency: new Currency(exRate.CurrencyCode),
                targetCurrency: new Currency(dailyExchangeRates.ExchangeRateProviderCurrencyCode),
                value: exRate.Rate / exRate.Amount);
            exchangeRates.Add(exchangeRate);
        }

        return exchangeRates;
    }
}
