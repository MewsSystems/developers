using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Domain.Common;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Api.Extensions;

public static class ExchangeRateExtensions
{
    public static ExchangeRateResponseDto ToExchangeRateResponse(
        this IEnumerable<ExchangeRate> exchangeRates,
        Maybe<DateOnly> requestedDate)
    {
        var rateList = exchangeRates.ToList();

        return new ExchangeRateResponseDto(
            Rates: rateList.Select(rate => new ExchangeRateDto(
                SourceCurrency: rate.SourceCurrency.Code,
                TargetCurrency: rate.TargetCurrency.Code,
                Value: rate.Value,
                Date: rate.Date.ToDateTime(TimeOnly.MinValue)
            )).ToList(),
            RequestedDate: requestedDate.TryGetValue(out var date) ? date.ToDateTime(TimeOnly.MinValue) : DateHelper.Today.ToDateTime(TimeOnly.MinValue),
            TotalCount: rateList.Count
        );
    }
}
