using ExchangeRateUpdater.Api.Application.Interfaces;
using ExchangeRateUpdater.Api.Domain;
using ExchangeRateUpdater.Api.Domain.Common;
using ExchangeRateUpdater.Api.Infrastructure.Interfaces;
using ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb.Models;

namespace ExchangeRateUpdater.Api.Application.Services;

public sealed class ExchangeRateProvider(
    IExternalExchangeRateClient externalClient,
    ILogger<ExchangeRateProvider> logger) : IExchangeRateProvider
{
    private const string TargetCurrencyCode = "CZK";

    public async Task<Result<IReadOnlyCollection<ExchangeRate>>> GetDailyRatesAsync(
        DateOnly? date = null,
        CancellationToken cancellationToken = default)
    {
        var externalResult = await externalClient.GetDailyRatesAsync(date, cancellationToken);

        if (externalResult.IsFailure)
            return Result<IReadOnlyCollection<ExchangeRate>>.Failure(externalResult.Error);

        var domainRates = new List<ExchangeRate>();

        foreach (var external in externalResult.Value.Rates)
        {
            var result = MapToDomainModel(external);
            if (result.IsFailure)
                return Result<IReadOnlyCollection<ExchangeRate>>.Failure(result.Error);

            domainRates.Add(result.Value);
        }

        logger.LogInformation(
            "Mapped {Count} exchange rates for date {Date}",
            domainRates.Count,
            date?.ToString("yyyy-MM-dd") ?? "latest");

        return Result<IReadOnlyCollection<ExchangeRate>>.Success(domainRates.ToArray());
    }

    private static Result<ExchangeRate> MapToDomainModel(CnbExRateDaily external) =>
        ExchangeRate.Create(
            external.CurrencyCode,
            TargetCurrencyCode,
            external.Rate,
            external.Amount,
            DateOnly.Parse(external.ValidFor));
}
