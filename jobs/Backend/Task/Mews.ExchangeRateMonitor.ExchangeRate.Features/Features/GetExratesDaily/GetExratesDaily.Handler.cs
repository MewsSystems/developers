using FluentValidation;
using Mews.ExchangeRateMonitor.Common.Application.Extensions;
using Mews.ExchangeRateMonitor.Common.Domain.Handlers;
using Mews.ExchangeRateMonitor.Common.Domain.Results;
using Microsoft.Extensions.Logging;

namespace Mews.ExchangeRateMonitor.ExchangeRate.Features.Features.GetExratesDaily;

public interface IGetExratesDailyHandler : IHandler
{
    Task<Result<IEnumerable<CurrencyExchangeRateDto>>> HandleAsync(GetExratesDailyRequest request, CancellationToken ct);
}

public sealed class GetExratesDailyHandler(
    ILogger<GetExratesDailyHandler> logger,
    IExchangeRateProvider exchangeRateProvider,
    IValidator<GetExratesDailyRequest> validator) : IGetExratesDailyHandler
{
    public async Task<Result<IEnumerable<CurrencyExchangeRateDto>>> HandleAsync(GetExratesDailyRequest request, CancellationToken ct)
    {
        logger.LogInformation($"{nameof(GetExratesDailyHandler)}.{nameof(HandleAsync)} started");

        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return validationResult.ToDomainErrors(nameof(GetExratesDailyRequest));

        var dailyRatesRes = await exchangeRateProvider.GetDailyRatesAsync(request.Date, ct);
        if (dailyRatesRes.IsError)
            return dailyRatesRes.Errors;

        var dailyRates = dailyRatesRes.Value ?? [];
        var rateDtos = dailyRates.Select(x => x.ToRateDto()).ToList();

        logger.LogInformation($"{nameof(GetExratesDailyHandler)}.{nameof(HandleAsync)} is successfull");

        return rateDtos;
    }
}