using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Commands.Currencies.DeleteCurrency;

public class DeleteCurrencyCommandHandler
    : ICommandHandler<DeleteCurrencyCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCurrencyCommandHandler> _logger;

    public DeleteCurrencyCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteCurrencyCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        var currency = await _unitOfWork.Currencies
            .GetByIdAsync(request.CurrencyId, cancellationToken);

        if (currency == null)
        {
            _logger.LogWarning("Currency {CurrencyId} not found", request.CurrencyId);
            return Result.Failure($"Currency with ID {request.CurrencyId} not found.");
        }

        try
        {
            // Check if currency is being used by providers
            var providers = await _unitOfWork.ExchangeRateProviders.GetAllAsync(cancellationToken);
            var providersUsingCurrency = providers.Where(p => p.BaseCurrencyId == currency.Id).ToList();

            if (providersUsingCurrency.Any() && !request.Force)
            {
                _logger.LogWarning(
                    "Cannot delete currency {Code} because it is used by {Count} provider(s)",
                    currency.Code,
                    providersUsingCurrency.Count);

                return Result.Failure(
                    $"Cannot delete currency '{currency.Code}' because it is used as base currency by {providersUsingCurrency.Count} provider(s). " +
                    "Use Force=true to delete anyway.");
            }

            // Check if currency is being used in exchange rates
            // Note: This is a simplified check. In production, you'd want database-level query for better performance
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var allCurrencies = await _unitOfWork.Currencies.GetAllAsync(cancellationToken);

            var hasRates = false;
            foreach (var otherCurrency in allCurrencies.Where(c => c.Id != currency.Id))
            {
                var rates = await _unitOfWork.ExchangeRates.GetHistoryAsync(
                    currency.Id,
                    otherCurrency.Id,
                    today.AddDays(-30),
                    today,
                    cancellationToken);

                if (rates.Any())
                {
                    hasRates = true;
                    break;
                }

                // Check reverse direction
                var reverseRates = await _unitOfWork.ExchangeRates.GetHistoryAsync(
                    otherCurrency.Id,
                    currency.Id,
                    today.AddDays(-30),
                    today,
                    cancellationToken);

                if (reverseRates.Any())
                {
                    hasRates = true;
                    break;
                }
            }

            if (hasRates && !request.Force)
            {
                _logger.LogWarning(
                    "Cannot delete currency {Code} because it has associated exchange rates",
                    currency.Code);

                return Result.Failure(
                    $"Cannot delete currency '{currency.Code}' because it has associated exchange rates. " +
                    "Use Force=true to delete anyway (this may orphan exchange rate data).");
            }

            // Delete currency
            await _unitOfWork.Currencies.DeleteAsync(currency, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted currency {Code}", currency.Code);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting currency {CurrencyId}", request.CurrencyId);
            return Result.Failure($"Failed to delete currency: {ex.Message}");
        }
    }
}
