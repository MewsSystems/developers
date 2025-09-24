using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Api.Extensions;

public static class ExchangeRateExtensions
{
    public static ExchangeRateResponse ToExchangeRateResponse(
        this IEnumerable<ExchangeRate> exchangeRates,
        DateTime requestedDate)
    {
        var rateList = exchangeRates.ToList();

        return new ExchangeRateResponse(
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
