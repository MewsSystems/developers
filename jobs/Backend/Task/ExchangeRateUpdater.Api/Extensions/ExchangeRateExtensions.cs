using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Api.Extensions;

public static class ExchangeRateExtensions
{
    public static ExchangeRateResponseDto ToExchangeRateResponse(
        this IEnumerable<ExchangeRate> exchangeRates,
        DateTime requestedDate)
    {
        var rateList = exchangeRates.ToList();

        return new ExchangeRateResponseDto(
            Rates: rateList.Select(rate => new ExchangeRateDto(
                SourceCurrency: rate.SourceCurrency.Code,
                TargetCurrency: rate.TargetCurrency.Code,
                Value: rate.Value,
                Date: rate.Date
            )).ToList(),
            RequestedDate: requestedDate,
            TotalCount: rateList.Count
        );
    }
}
